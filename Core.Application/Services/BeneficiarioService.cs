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
        private readonly AuthenticationResponse _user;

        public BeneficiarioService(IBeneficiarioRepository beneficiario, IMapper mapper) : base(beneficiario, mapper)
        {
            _beneficiarioRepository = beneficiario;
            _mapper = mapper;
        }

        public async Task<List<BeneficiarioViewModel>> GetAllViewModelWithInclude(string _user)
        {
            var beneficiariosList = await _beneficiarioRepository.GetAllWithInclude(new List<string> { "CuentaAhorro" });

            List<BeneficiarioViewModel> beneficiarioMapped = _mapper.Map<List<BeneficiarioViewModel>>(beneficiariosList);

            beneficiarioMapped = beneficiarioMapped.Where(p => p.IdUser == _user).Select(beneficiaro => new BeneficiarioViewModel
            {
                NumeroCuenta = beneficiaro.NumeroCuenta,
                IdBeneficiario = beneficiaro.IdBeneficiario
            }).ToList();

            List<BeneficiarioViewModel> listX = new();

            foreach (var beneficiario in beneficiarioMapped)
            {
                //var accountUser =  _accountExtensionService.FindAccountUserById(beneficiario.IdBeneficiario);

                var beneficiarioVM = new BeneficiarioViewModel();

                //beneficiarioVM.FirstName = accountUser.FirstName;
                //beneficiarioVM.LastName = accountUser.UserName;
                beneficiarioVM.NumeroCuenta = beneficiario.NumeroCuenta;

                listX.Add(beneficiarioVM);

            }

            

            return listX;
        }

    }
}
