using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.ViewModels.Products;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    internal class TarjetaCreditoService : GenericService<SaveTarjetaCreditoViewModel, TarjetaCreditoViewModel, TarjetaCredito>, ITarjetaCreditoService
    {
        private readonly ITarjetaCreditoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository; 
        private readonly AuthenticationResponse authenticationResponse;
        private readonly IHttpContextAccessor _accessor;

        public TarjetaCreditoService(ITarjetaCreditoRepository tarjetaCreditoRepository, IMapper mapper, IProductService service,
            IProductRepository productRepository, IHttpContextAccessor accessor) : base(tarjetaCreditoRepository, mapper)
        {
            _repository = tarjetaCreditoRepository;
            _mapper = mapper;
            _productService = service;
            _productRepository = productRepository;
            _accessor = accessor; 
            authenticationResponse = _accessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<SaveTarjetaCreditoViewModel> AddTarjetaCredito(SaveTarjetaCreditoViewModel vm)
        {
            string numeroCuenta = "";

            for (int i = 1; i < 11; i++)
            {
                var randomNumeroCuenta = new Random();

                numeroCuenta += String.Join("",randomNumeroCuenta.Next(0, 10).ToString());

            }

            vm.NumeroTarjeta = numeroCuenta;
            
            var productVm = new SaveProductViewModel();
            
            productVm.IdProductType = null;
            productVm.isCuentaAhorro = false;
            productVm.isTarjetaCredito = true;
            productVm.Primary = false;
            productVm.isPrestamo = false;
            productVm.IdUser = vm.UserId;
            
            
            SaveProductViewModel productResult = await _productService.Add(productVm);

            TarjetaCredito tarjeta = _mapper.Map<TarjetaCredito>(vm);
            tarjeta.IdProduct = productResult.Id;
            TarjetaCredito resultado = await _repository.AddAsync(tarjeta);
            Product producto = _mapper.Map<Product>(productResult);


            producto.IdProductType = resultado.Id;
            producto.CreatedBy = authenticationResponse.Id;
            resultado.IdProduct = productResult.Id;

            await _repository.UpdateAsync(resultado, resultado.Id);
            await _productRepository.UpdateAsync(producto, producto.Id);

            return _mapper.Map<SaveTarjetaCreditoViewModel>(resultado);
        }
    }
}
