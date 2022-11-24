using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Helpers;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Services
{
    public class PrestamoService : GenericService<SavePrestamoViewModel, PrestamoViewModel, Prestamo>, IPrestamoService
    {
        private readonly IPrestamoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _accessor;
        private readonly AuthenticationResponse authenticationResponse;
        private readonly IProductRepository _productRepository;
        
        public PrestamoService(IPrestamoRepository prestamoRepository, IMapper mapper, IProductService repository,
            IHttpContextAccessor accessor, IProductRepository productRepository) : base(prestamoRepository, mapper)
        {
            _repository = prestamoRepository;
            _mapper = mapper;
            _productService = repository;
            _accessor = accessor;
            _productRepository = productRepository;
            authenticationResponse = _accessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<SavePrestamoViewModel> AddCuentaAhorro( SavePrestamoViewModel prestamoVm, string userId )
        {
            string numeroCuenta = "";

            for (int i = 1; i < 11; i++)
            {
                var randomNumeroCuenta = new Random();

                numeroCuenta += String.Join("",randomNumeroCuenta.Next(0, 10).ToString());
            }

            prestamoVm.NumeroPrestamo = numeroCuenta;

            var productVm = new SaveProductViewModel();
            
            productVm.IdProductType = null;
            productVm.isCuentaAhorro = false;
            productVm.isTarjetaCredito = false;
            productVm.Primary = false;
            productVm.isPrestamo = true;
            productVm.IdUser = userId;
            
            SaveProductViewModel productResult = await _productService.Add(productVm);

            Prestamo prestamo = _mapper.Map<Prestamo>(prestamoVm);
            prestamo.IdProduct = productResult.Id;
            Prestamo resultado = await _repository.AddAsync(prestamo);
            
            Product producto = _mapper.Map<Product>(productResult);

            producto.IdProductType = resultado.Id;
            producto.CreatedBy = authenticationResponse.Id;
            resultado.IdProduct = productResult.Id;
            resultado.Debe = prestamo.Monto;

            await _repository.UpdateAsync(resultado, resultado.Id);
            await _productRepository.UpdateAsync(producto, producto.Id);

            return _mapper.Map<SavePrestamoViewModel>(resultado);
        }


        public async Task<List<PrestamoViewModel>> GetAllPrestamosById(string id)
        {
            var products = await _productService.GetAllViewModelWithIncludeById(id);

            return products.Where(p => p.isPrestamo == true).Select(p => new PrestamoViewModel
            {
                Id = p.Prestamos.Id,
                NumeroPrestamo = p.Prestamos.NumeroPrestamo,
                Monto = p.Prestamos.Monto,
                Pago = p.Prestamos.Pago,
                Debe = p.Prestamos.Debe,
                Balance = p.Prestamos.Balance,
                IdProduct = p.Prestamos.IdProduct,
                Product = p.Prestamos.Product
            }).ToList();
        }

    }
}
