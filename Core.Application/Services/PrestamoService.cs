using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Prestamo;
using Core.Application.ViewModels.User;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
