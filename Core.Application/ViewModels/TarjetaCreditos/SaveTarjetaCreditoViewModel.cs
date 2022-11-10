using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.TarjetaCreditos
{
    public class SaveTarjetaCreditoViewModel
    {
        //faltan verificaciones ( en espera )
        public string NumeroTarjeta { get; set; }

        public double Limite { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } // 1, 0

        public double Balance { get; set; }

        public int IdProduct { get; set; }
    }
}
