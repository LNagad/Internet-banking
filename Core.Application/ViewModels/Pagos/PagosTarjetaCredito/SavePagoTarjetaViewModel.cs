using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Pagos.PagosTarjetaCredito
{
    public class SavePagoTarjetaViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
        public string NumeroCuentaOrigen { get; set; }

        
        [Required(ErrorMessage = "Debe ingresar el monto que desea pagar")]

        [DataType(DataType.Text)]
        public string Monto { get; set; }

        public string idProduct { get; set; }
        public bool? HasError { get; set; }
        public string? Error { get; set; }
    }
}
