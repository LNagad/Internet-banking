using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;

namespace Core.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public int IdProductType { get; set; }

        public int IdUser { get; set; }

        public int Primary { get; set; } // 1 = True, 0 = False

        //Navigation properties
        public UserViewModel User { get; set; }
        public List<CuentaAhorroViewModel> CuentaAhorros { get; set; }
        public List<TarjetaCreditoViewModel> TarjetaCreditos { get; set; }
        public List<PrestamoViewModel> Prestamos { get; set; }
    }
}
