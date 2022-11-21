using AutoMapper;
using Core.Application.Dtos.Account;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Beneficiarios;
using Core.Application.ViewModels.Products;
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
        private readonly IDashboradService _dashboradService;

        public BeneficiarioService(IBeneficiarioRepository beneficiario, IMapper mapper, IDashboradService dashboradService) : base(beneficiario, mapper)
        {
            _beneficiarioRepository = beneficiario;
            _mapper = mapper;
            _dashboradService = dashboradService;
        }

        public async Task<List<BeneficiarioViewModel>> GetAllViewModelWithInclude(string _user)
        {
            var beneficiariosList = await _beneficiarioRepository.GetAllWithInclude(new List<string> { "CuentaAhorro" });

            List<BeneficiarioViewModel> beneficiarioMapped = _mapper.Map<List<BeneficiarioViewModel>>(beneficiariosList);

            //beneficiarioMapped = beneficiarioMapped.Where(p => p.IdUser == _user).Select(beneficiaro => new BeneficiarioViewModel
            //{
            //    NumeroCuenta = beneficiaro.NumeroCuenta,
            //    IdBeneficiario = beneficiaro.IdBeneficiario
            //}).ToList();

            List<BeneficiarioViewModel> listX = new();

            foreach (var beneficiario in beneficiarioMapped)
            {
                var userX = await _dashboradService.getUserAndInformation(beneficiario.IdBeneficiario);

                var beneficiarioVM = new BeneficiarioViewModel();

                beneficiarioVM.FirstName = userX.FirstName;
                beneficiarioVM.LastName = userX.LastName;
                beneficiarioVM.NumeroCuenta = beneficiario.NumeroCuenta;

                listX.Add(beneficiarioVM);
            }

            return listX;
        }

    }
}
