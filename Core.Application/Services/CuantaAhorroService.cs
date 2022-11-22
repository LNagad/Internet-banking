using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    internal class CuantaAhorroService : GenericService<SaveCuentaAhorroViewModel, CuentaAhorroViewModel, CuentaAhorro>, ICuentaAhorroService
    {
        private readonly ICuentaAhorroRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly AuthenticationResponse authenticationResponse;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepo;

        public CuantaAhorroService(ICuentaAhorroRepository CuentaAhorroRepository, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IProductService service, IProductRepository productRepository) : base(CuentaAhorroRepository, mapper)
        {
            _repository = CuentaAhorroRepository;
            _mapper = mapper;
            _accessor = contextAccessor;
            _productService = service; 
            _productRepo = productRepository;
            authenticationResponse = _accessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }


        public async Task<CuentaAhorroViewModel> AccountExists(string NumeroCuenta)
        {
            CuentaAhorro cuenta = await _repository.AccountExists(NumeroCuenta);

            if (cuenta != null)
            {
                cuenta.Product = await _productRepo.GetByIdAsync(cuenta.IdProduct);

                return _mapper.Map<CuentaAhorroViewModel>(cuenta);
            }

            return null;

        }


          public async Task<List<CuentaAhorroViewModel>> GetAllViewModelWithInclude(string user = null)
        {
            var cuentasList = await _repository.GetAllWithInclude(new List<string> { "Product", "Beneficiarios" });

            List<CuentaAhorroViewModel> cuentasMapped = _mapper.Map<List<CuentaAhorroViewModel>>(cuentasList);

            if (user != null )
            {
                return cuentasMapped = cuentasMapped.Where(p => p.Product.IdUser == user).Select(cuenta => new CuentaAhorroViewModel
                {
                   Id = cuenta.Id,
                   NumeroCuenta = cuenta.NumeroCuenta,
                   Balance = cuenta.Balance,
                   Principal = cuenta.Principal,
                   IdProduct = cuenta.IdProduct,
                   Product = cuenta.Product,
                   Beneficiarios = cuenta.Beneficiarios
                }).ToList();

            }

            return cuentasMapped = cuentasMapped.Select(cuenta => new CuentaAhorroViewModel
            {
                Id = cuenta.Id,
                NumeroCuenta = cuenta.NumeroCuenta,
                Balance = cuenta.Balance,
                Principal = cuenta.Principal,
                IdProduct = cuenta.IdProduct,
                Product = cuenta.Product,
                Beneficiarios = cuenta.Beneficiarios
            }).ToList();

        }

      public async Task<SaveCuentaAhorroViewModel> Add( SaveCuentaAhorroViewModel vm, string userId, bool primary )
        {
            string numeroCuenta = "";

            for (int i = 1; i < 11; i++)
            {
                var randomNumeroCuenta = new Random();

                numeroCuenta += String.Join("",randomNumeroCuenta.Next(0, 10).ToString());

            }

            vm.NumeroCuenta = numeroCuenta;
            
            if (primary)
            {
                vm.Principal = true;
            }
            else
            {
                vm.Principal = false;
            }

            CuentaAhorro cuenta = _mapper.Map<CuentaAhorro>(vm);
            
            CuentaAhorro resultado = await _repository.AddAsync(cuenta);

            var productVM = new SaveProductViewModel();

            if (resultado != null)
            {
                productVM.IdProductType = resultado.Id;
                productVM.isCuentaAhorro = true;
                productVM.isTarjetaCredito = false;
                productVM.Primary = false;
                productVM.isPrestamo = false;

                productVM.IdUser = userId;
            }

            SaveProductViewModel productResult = await _productService.Add(productVM);

            if (productResult != null)
            {
                resultado.IdProduct = productResult.Id;
             
                await _repository.UpdateAsync(resultado, resultado.Id);
            }
            
            
            return _mapper.Map<SaveCuentaAhorroViewModel>(resultado);
        }
    }
}
