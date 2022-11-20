using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    internal class CuantaAhorroService : GenericService<SaveCuentaAhorroViewModel, CuentaAhorroViewModel, CuentaAhorro>, ICuentaAhorroService
    {
        private readonly ICuentaAhorroRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProductRepository productService;
        public CuantaAhorroService(ICuentaAhorroRepository CuentaAhorroRepository, IMapper mapper, IProductRepository productService) 
            : base(CuentaAhorroRepository, mapper)
        {
            _repository = CuentaAhorroRepository;
            _mapper = mapper;
            this.productService = productService;
        }


        public async Task<CuentaAhorroViewModel> AccountExists(string NumeroCuenta)
        {
            CuentaAhorro cuenta = await _repository.AccountExists(NumeroCuenta);

            if (cuenta != null)
            {
                cuenta.Product = await productService.GetByIdAsync(cuenta.IdProduct);

                return _mapper.Map<CuentaAhorroViewModel>(cuenta);
            }

            return null;

        }
    }
}
