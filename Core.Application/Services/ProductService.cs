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

        public ProductService(IProductRepository repository, 
            IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
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

        public async Task<List<Product>> GetAllTransactions()
        {
            Product product = new();
            var products = await _repository.GetAllAsync();
            List<Product> listProducts = new List<Product>();

            foreach ( var item in products.Where(x=>x.Created.ToString() == "2016-12-01 00:00:00.000"))
            {
                listProducts.Add( item );
            }

            return listProducts;
        }
    }
}
