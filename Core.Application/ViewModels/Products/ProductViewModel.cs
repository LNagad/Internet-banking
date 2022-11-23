using Core.Application.Dtos.Account;
using Core.Application.ViewModels.CuentaAhorros;
using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.TarjetaCreditos;
using Core.Application.ViewModels.Users;
using Core.Domain.Entities;

namespace Core.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string IdProductType { get; set; }

        public bool? isCuentaAhorro { get; set; }

        public bool? isTarjetaCredito { get; set; }

        public bool? isPrestamo { get; set; }

        public string? IdUser { get; set; }

        public bool? Primary { get; set; }

        public AuthenticationResponse User { get; set; }

        //Navigation properties
        public CuentaAhorroViewModel CuentaAhorros { get; set; }
        public TarjetaCreditoViewModel TarjetaCreditos { get; set; }
        public PrestamoViewModel Prestamos { get; set; }
    }
}
