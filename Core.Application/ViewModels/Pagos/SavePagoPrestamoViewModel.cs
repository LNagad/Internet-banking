using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Pagos
{
    public class SavePagoPrestamoViewModel
    {

        [Required(ErrorMessage = "Debe ingresar el monto que desea pagar")]

        [DataType(DataType.Text)]
        public double Monto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
        public string NumeroCuentaOrigen { get; set; }

        public string idProduct { get; set; }

        public bool? HasError { get; set; }
        public string? Error { get; set; }
    }
}
