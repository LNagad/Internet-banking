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

        public CuantaAhorroService(ICuentaAhorroRepository CuentaAhorroRepository, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IProductService service) : base(CuentaAhorroRepository, mapper)
        {
            _repository = CuentaAhorroRepository;
            _mapper = mapper;
            _accessor = contextAccessor;
            _productService = service; 
            
            authenticationResponse = _accessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


        public async Task<CuentaAhorroViewModel> AccountExists(string NumeroCuenta)
        {
            CuentaAhorro cuenta = await _repository.AccountExists(NumeroCuenta);

            if (cuenta != null)
            {
                return _mapper.Map<CuentaAhorroViewModel>(cuenta);
            }

            return null;

        }

        public async Task<SaveCuentaAhorroViewModel> Add( SaveCuentaAhorroViewModel vm, string userId )
        {
            string numeroCuenta = "";

            for (int i = 1; i < 11; i++)
            {
                var randomNumeroCuenta = new Random();

                numeroCuenta += String.Join("",randomNumeroCuenta.Next(0, 10).ToString());

            }

            vm.NumeroCuenta = numeroCuenta;
            vm.Principal = true;

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
