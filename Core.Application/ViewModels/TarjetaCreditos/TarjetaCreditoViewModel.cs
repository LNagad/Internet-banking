﻿using Core.Application.ViewModels.Products;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.TarjetaCreditos
{
    public class TarjetaCreditoViewModel
    {
        public string Id { get; set; }

        public string NumeroTarjeta { get; set; }

        public double Limite { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } // 1, 0

        //navigation property
        public string Idproduct { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
