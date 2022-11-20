using Core.Application.ViewModels.Beneficiarios;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface IBeneficiarioService : IGenericService<SaveBeneficiarioViewModel, BeneficiarioViewModel ,Beneficiario>
    {
        Task<List<BeneficiarioViewModel>> GetAllViewModelWithInclude(string _user);
    }
}
