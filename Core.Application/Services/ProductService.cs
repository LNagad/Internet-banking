using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;
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
        private readonly ICuentaAhorroRepository _cuentaAhorroRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ITarjetaCreditoRepository _tarjetaCreditoRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(
            IProductRepository repository,IMapper mapper, ICuentaAhorroRepository cuentaAhorroRepository, IPrestamoRepository prestamoRepository, ITarjetaCreditoRepository tarjetaCreditoRepository, IUserRepository userRepository) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _cuentaAhorroRepository = cuentaAhorroRepository;
            _prestamoRepository = prestamoRepository;
            _tarjetaCreditoRepository = tarjetaCreditoRepository;
            _userRepository = userRepository;
        }

        public async Task<List<ProductViewModel>> GetAllViewModelWithInclude()
        {
            var cuentaAhorroVm = await _cuentaAhorroRepository.GetAllAsync();
            List<CuentaAhorroViewModel> cuentaAhorroList = _mapper.Map<List<CuentaAhorroViewModel>>(cuentaAhorroVm);

            var prestamoVm = await _prestamoRepository.GetAllAsync();
            List<PrestamoViewModel> prestamoList = _mapper.Map<List<PrestamoViewModel>>(prestamoVm);

            var tarjetaCreditoVm = await _tarjetaCreditoRepository.GetAllAsync();
            List<TarjetaCreditoViewModel> tarjetaCreditoList = _mapper.Map<List<TarjetaCreditoViewModel>>(tarjetaCreditoVm);

            var productList = await _repository.GetAllWithInclude(new List<string> { });
            return productList.Select(product => new ProductViewModel { 
                Id = product.Id,
                IdProduct = product.IdProduct,
                IdProductType = product.IdProductType,
                IdUser = product.IdUser,
                Primary = product.Primary,
                CuentaAhorros = cuentaAhorroList.Where(x => x.IdProduct == product.IdProduct).ToList(),
                Prestamos = prestamoList.Where(x => x.IdProduct == product.IdProduct).ToList(),
                TarjetaCreditos = tarjetaCreditoList.Where(x => x.Idproduct == product.IdProduct).ToList()
            }).ToList();
        }
    }
}
