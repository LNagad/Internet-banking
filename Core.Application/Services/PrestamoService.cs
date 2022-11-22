using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Prestamos;
using Core.Domain.Entities;

namespace Core.Application.Services
{
    public class PrestamoService : GenericService<SavePrestamoViewModel, PrestamoViewModel, Prestamo>, IPrestamoService
    {
        private readonly IPrestamoRepository _repository;
        private readonly IMapper _mapper;

        public PrestamoService(IPrestamoRepository prestamoRepository, IMapper mapper) : base(prestamoRepository, mapper)
        {
            _repository = prestamoRepository;
            _mapper = mapper;
        }
        
        
    }
}
