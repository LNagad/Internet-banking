using Core.Application.ViewModels.Beneficiarios;
using Core.Application.ViewModels.Products;

namespace Core.Application.ViewModels.CuentaAhorros
{
    public class CuentaAhorroViewModel
    {
        public string NumeroCuenta { get; set; }

        public double Balance { get; set; }

        public int Principal { get; set; }

        //navigation property
        public int IdProduct { get; set; }
        public ProductViewModel Product { get; set; }
        public List<BeneficiarioViewModel> Beneficiarios { get; set; }
    }
}
