using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Beneficiarios;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class BeneficiarioService : GenericService<SaveBeneficiarioViewModel, BeneficiarioViewModel, Beneficiario>, IBeneficiarioService
    {
        private readonly IBeneficiarioRepository _beneficiarioRepository;
        private readonly IMapper _mapper;

        public BeneficiarioService(IBeneficiarioRepository beneficiario, IMapper mapper) : base(beneficiario, mapper)
        {
            _beneficiarioRepository = beneficiario;
            _mapper = mapper;
        }

    }
}
