using Core.Application.ViewModels.CuentaAhorros;

namespace Core.Application.ViewModels.Beneficiarios
{
    public class BeneficiarioViewModel
    {
        public int Id { get; set; }

        public int IdAccount { get; set; }

        public int IdUser { get; set; }

        public int IdBeneficiario { get; set; }

        //Navigation property
        public CuentaAhorroViewModel CuentaAhorro { get; set; }
    }
}
