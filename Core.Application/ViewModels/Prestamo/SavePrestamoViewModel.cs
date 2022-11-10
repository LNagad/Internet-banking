using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Prestamo
{
    public class SavePrestamoViewModel
    {
        //faltan verificaciones
        public string NumeroPrestamo { get; set; }

        public double Monto { get; set; }

        public double Pago { get; set; }

        public int Debe { get; set; } //1 , 0

        public double Balance { get; set; }
    }
}
