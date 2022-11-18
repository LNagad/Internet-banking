using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Products
{
    public class SaveProductViewModel
    {

        public string Id { get; set; }

        public string? IdProductType { get; set; }

        public bool? isCuentaAhorro { get; set; }

        public bool? isTarjetaCredito { get; set; }

        public bool? isPrestamo { get; set; }

        public string? IdUser { get; set; }

        public bool? Primary { get; set; } // 1 = True, 0 = False
    }
}
