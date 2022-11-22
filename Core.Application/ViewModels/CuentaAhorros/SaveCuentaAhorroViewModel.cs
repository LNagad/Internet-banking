using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.CuentaAhorros
{
    public class SaveCuentaAhorroViewModel
    {
        public string? Id { get; set; }

        public string? NumeroCuenta { get; set; }

        public double Balance { get; set; }

        public bool? Principal { get; set; }

        public string? IdProduct { get; set; }
        
        public string? UserId { get; set; }
    }
}
