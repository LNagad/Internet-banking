﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.TarjetaCreditos
{
    public class SaveTarjetaCreditoViewModel
    {

        public string? Id { get; set; }

        public string NumeroTarjeta { get; set; }

        public double Limite { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } // 1, 0

        public double Balance { get; set; }

        public string? IdProduct { get; set; }

        public string UserId { get; set; }
    }
}
