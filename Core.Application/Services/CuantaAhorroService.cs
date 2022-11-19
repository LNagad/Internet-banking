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

        public CuantaAhorroService(ICuentaAhorroRepository CuentaAhorroRepository, IMapper mapper) : base(CuentaAhorroRepository, mapper)
        {
            _repository = CuentaAhorroRepository;
            _mapper = mapper;
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
    }
}
