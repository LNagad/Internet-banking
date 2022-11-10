using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.TarjetaCredito
{
    public class TarjetaCreditoViewModel
    {
        public string NumeroTarjeta { get; set; }

        public double Limite { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } // 1, 0

        public double Balance { get; set; }
    }
}
