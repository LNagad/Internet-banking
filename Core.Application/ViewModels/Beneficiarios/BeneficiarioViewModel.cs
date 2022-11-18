using Core.Application.ViewModels.CuentaAhorros;

namespace Core.Application.ViewModels.Beneficiarios
{
    public class BeneficiarioViewModel
    {
        public string Id { get; set; }

        public string IdAccount { get; set; }

        public string IdUser { get; set; }

        public string IdBeneficiario { get; set; }

        //Navigation property
        public CuentaAhorroViewModel CuentaAhorro { get; set; }
    }
}
