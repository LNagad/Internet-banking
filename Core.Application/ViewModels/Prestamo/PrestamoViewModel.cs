using Core.Domain.Entities;

namespace Core.Application.ViewModels.Prestamo
{
    public class PrestamoViewModel
    {
        public string NumeroPrestamo { get; set; }

        public double Monto { get; set; }

        public double Pago { get; set; }

        public int Debe { get; set; } //1 , 0

        public double Balance { get; set; }
    }
}
