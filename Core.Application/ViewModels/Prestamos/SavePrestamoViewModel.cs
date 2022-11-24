using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Prestamos
{
    public class SavePrestamoViewModel
    {

        public string? Id { get; set; }

        public string NumeroPrestamo { get; set; }

        public double Monto { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } 

        public double Balance { get; set; }
        public string IdProduct { get; set; }
    }
}
