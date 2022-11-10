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

namespace Core.Application.Services
{
    internal class TarjetaCreditoService : GenericService<SaveTarjetaCreditoViewModel, TarjetaCreditoViewModel, TarjetaCredito>, ITarjetaCreditoService
    {
        private readonly ITarjetaCreditoRepository _repository;
        private readonly IMapper _mapper;

        public TarjetaCreditoService(ITarjetaCreditoRepository tarjetaCreditoRepository, IMapper mapper) : base(tarjetaCreditoRepository, mapper)
        {
            _repository = tarjetaCreditoRepository;
            _mapper = mapper;
        }
    }
}
