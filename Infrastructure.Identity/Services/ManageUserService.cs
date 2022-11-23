using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
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
    private readonly ITarjetaCreditoService _tarjetaCreditoService; 

    public ManageUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IEmailService emailService, ICuentaAhorroService service,  IMapper mapper, IProductService product
        ,ITarjetaCreditoService serviceTarjeta)
    {
        _userManager = userManager;
        _cuentaAhorro = service;
        _mapper = mapper;
        _product = product;
        _tarjetaCreditoService = serviceTarjeta;
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
}