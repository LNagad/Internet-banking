using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.CuentaAhorro
{
    public class SaveCuentaAhorroViewModel
    {
        //faltan verificaciones ( en espera de analisis )
        public string NumeroCuenta { get; set; }

        public double Balance { get; set; }

        public int Principal { get; set; }
    }
}
