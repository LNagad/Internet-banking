using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Pagos.PagosExpresos
{
    public class SavePagoExpresoViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
        public string NumeroCuentaOrigen { get; set; }

        [Required(ErrorMessage = "Debe ingresar la cuenta de destino")]
        public string NumeroCuentaDestino { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto que desea enviar")]

        [DataType(DataType.Text)]
        public string Monto { get; set; }

        public bool? HasError { get; set; }
        public string? Error { get; set; }
    }
}
