using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class ManageUserService : IManageUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICuentaAhorroService _cuentaAhorro;
    private readonly IMapper _mapper;
    private readonly IProductService _product;
    private readonly IProductRepository _productRepository;
    private readonly ITarjetaCreditoService _tarjetaCreditoService;
    private readonly ICuentaAhorroRepository _ahorroRepository;
    private readonly IPrestamoService _prestamoService;
    
    public ManageUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IEmailService emailService, ICuentaAhorroService service,  IMapper mapper, IProductService product
        ,ITarjetaCreditoService serviceTarjeta, IProductRepository productRepository, 
        ICuentaAhorroRepository ahorroRepository, IPrestamoService servicePrestamo)
    {
        _userManager = userManager;
        _cuentaAhorro = service;
        _mapper = mapper;
        _product = product;
        _tarjetaCreditoService = serviceTarjeta;
        _productRepository = productRepository;
        _ahorroRepository = ahorroRepository;
        _prestamoService = servicePrestamo;
    }

    public async Task<List<ProductViewModel>> gettinProductsById( string Id )
    {
        
        var userGot = await _userManager.FindByIdAsync( Id );

        AuthenticationResponse userAuth = new()
        {
            Id = userGot.Id,
            FirstName = userGot.FirstName,
            Email = userGot.Email,
            Status = userGot.Status,
        };

        List<ProductViewModel> getList = await _product.GetAllViewModelWithIncludeById(userAuth.Id);

        return getList; 
    }

    public async Task<string> ManageTarjetaCredito( SaveTarjetaCreditoViewModel tarjetaVm )
    {
        var userGot = await _userManager.FindByIdAsync(tarjetaVm.UserId);

        tarjetaVm.Debe = tarjetaVm.Limite;

        var something = await _tarjetaCreditoService.AddTarjetaCredito(tarjetaVm);
    
        return ""; 
    }

    public async Task<string> DeletingCuentaAhorro( string Id, double MontoCuenta, string tipo)
    {
        var productGot = await _productRepository.GetByIdAsync(Id); //busca el producto por Id
        var userGot = await _userManager.FindByIdAsync( productGot.IdUser ); //obtenemos el usuario al que pertence el producto
        List<ProductViewModel> getList = await _product.GetAllViewModelWithIncludeById(userGot.Id); //obtenemos una lista de todos los productos de ese usuario

        SavePrestamoViewModel prestamoVm = new();
        SaveTarjetaCreditoViewModel tarjetaVm = new();
        SaveCuentaAhorroViewModel ahorro = new SaveCuentaAhorroViewModel();
        
        if (tipo == "Ahorro")
        {
            foreach (ProductViewModel item in getList)
            {
                if (item.CuentaAhorros != null)
                {
                    if (item.CuentaAhorros.Principal == true)
                    {
                        ahorro = _mapper.Map<SaveCuentaAhorroViewModel>(item.CuentaAhorros);
                        ahorro.UserId = userGot.Id;
                        ahorro.Balance = item.CuentaAhorros.Balance + MontoCuenta;
                    }
                }
            }

            if (ahorro.Id != null)
            {
                await _cuentaAhorro.Update(ahorro, ahorro.Id);
            }
            
            await _product.Delete(Id);
        }
        else if ( tipo == "Tarjeta")
        {
            foreach( ProductViewModel item in getList )
            {
                if ( item.TarjetaCreditos != null )
                {
                    if ( item.TarjetaCreditos.Id == productGot.IdProductType )
                    {
                        tarjetaVm = _mapper.Map<SaveTarjetaCreditoViewModel>(item.TarjetaCreditos);

                        if (tarjetaVm.Debe == 0)
                        {
                            await _product.Delete(productGot.Id);
                        }

                        return "";
                    }
                }
            }
        }
        else if ( tipo == "Prestamo")
        {
            foreach (ProductViewModel item in getList)
            {
                if ( item.Prestamos != null )
                {
                    if ( item.Prestamos.Id == productGot.IdProductType )
                    {
                        prestamoVm = _mapper.Map<SavePrestamoViewModel>(item.Prestamos);

                        if ( prestamoVm.Debe == 0 )
                        {
                            await _product.Delete(productGot.Id);
                        }

                        return "";
                    }
                }
            }
        }

        return "";
    }

    public async Task<SavePrestamoViewModel> addPrestamoToUser(SavePrestamoViewModel prestamoVm, string Id)
    {
        var prestamo = new SavePrestamoViewModel();
        double MontoPrestamo = prestamoVm.Monto;
        
        var productGot = await _productRepository.GetByIdAsync(prestamoVm.IdProduct);
        var userGot = await _userManager.FindByIdAsync( productGot.IdUser ); 
        List<ProductViewModel> getList = await _product.GetAllViewModelWithIncludeById(userGot.Id);
        
        SaveCuentaAhorroViewModel ahorro = new SaveCuentaAhorroViewModel();

        foreach (var item in getList)
        {
            if (item.CuentaAhorros != null)
            {
                if (item.CuentaAhorros.Principal == true)
                {
                    ahorro = _mapper.Map<SaveCuentaAhorroViewModel>(item.CuentaAhorros);
                    ahorro.UserId = userGot.Id;
                    ahorro.Balance = item.CuentaAhorros.Balance + MontoPrestamo;
                }
            }
        }

        if (ahorro.Id != null)
        {
            await _cuentaAhorro.Update(ahorro, ahorro.Id);
            //await _prestamoService.AddCuentaAhorro(prestamoVm, productGot.IdUser);
        }
        
        return prestamo;
    }

    public async Task<SaveUserViewModel> GetSaveuserViewModelById(string id)
    {
        var user = new SaveUserViewModel();
        var userGot = await _userManager.FindByIdAsync(id);

        if ( userGot.Id != null)
        {
            user.Id = userGot.Id;
            user.Cedula = userGot.Cedula;
            user.Email = userGot.Email;
            user.Phone = userGot.PhoneNumber;
            user.LastName = userGot.LastName;
            user.FirstName = userGot.FirstName;
            user.UserName = userGot.UserName;
        }

        return user;
    }

    public async Task<string> UpdateUser(SaveUserViewModel vm)
    {
        var userApplication = new ApplicationUser();
        var userGot = await _userManager.FindByIdAsync(vm.Id);

        if (vm.Id != null)
        {
            userGot.Id = vm.Id;
            userGot.FirstName = vm.FirstName;
            userGot.Cedula = vm.Cedula;
            userGot.LastName = vm.LastName;
            userGot.Email = vm.Email;
            userGot.PhoneNumber = vm.Phone;
            userGot.UserName = vm.UserName;
        }

        if ( userApplication.Id != null)
        {
            await _userManager.UpdateAsync(userGot);
        }

        return "";
    }
}