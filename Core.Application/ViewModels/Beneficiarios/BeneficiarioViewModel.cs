using Core.Application.ViewModels.CuentaAhorros;
using Core.Domain.Entities;

namespace Core.Application.ViewModels.Beneficiarios
{
    public class BeneficiarioViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NumeroCuenta { get; set; }

        public string IdUser { get; set; }

        public string IdBeneficiario { get; set; }

        public CuentaAhorroViewModel CuentaAhorro { get; set; }
    }
}
