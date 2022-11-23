using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _user;
        private readonly IDashboradService _dashboardService;

        public ProductService(IProductRepository repository, 
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDashboradService dashboardService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _dashboardService = dashboardService;
        }

        public async Task<List<ProductViewModel>> GetAllViewModelWithInclude()
        {
            var productListX = await _repository.GetAllWithInclude(new List<string> { "CuentaAhorros", "TarjetaCreditos", "Prestamos" });

            List<ProductViewModel> productMapped = _mapper.Map<List<ProductViewModel>>(productListX);

            return productMapped = productMapped.Where(p => p.IdUser == _user.Id).Select(product => new ProductViewModel
            {
                Id = product.Id,
                IdProductType = product.IdProductType,
                IdUser = product.IdUser,
                Primary = product.Primary,
                CuentaAhorros = product.CuentaAhorros,
                Prestamos = product.Prestamos,
                TarjetaCreditos = product.TarjetaCreditos,
                User = _user,
            }).ToList();
        }
        
        public async Task<List<ProductViewModel>> GetAllViewModelWithIncludeById( string id )
        {
            var productListX = await _repository.GetAllWithInclude(new List<string> { "CuentaAhorros", "TarjetaCreditos", "Prestamos" });

            var user = await _dashboardService.getUserAndInformation(id);

            List<ProductViewModel> productMapped = _mapper.Map<List<ProductViewModel>>(productListX);

            return productMapped = productMapped.Where(p => p.IdUser == user.Id).Select(product => new ProductViewModel
            {
                Id = product.Id,
                IdProductType = product.IdProductType,
                IdUser = product.IdUser,
                Primary = product.Primary,
                CuentaAhorros = product.CuentaAhorros,
                Prestamos = product.Prestamos,
                TarjetaCreditos = product.TarjetaCreditos,
                User = user,
                isCuentaAhorro = product.isCuentaAhorro,
                isTarjetaCredito = product.isTarjetaCredito,
                isPrestamo = product.isPrestamo
            }).ToList();
        }


        public async Task<ProductViewModel> ProductExist(string productId)
        {
            Product cuenta = await _repository.AccountExists(productId);

            if (cuenta != null)
            {
                return _mapper.Map<ProductViewModel>(cuenta);
            }

            return null;

        }
    }
}
