
using System.ComponentModel.DataAnnotations;


namespace Core.Application.ViewModels.Pagos
{
    public class SavePagoEntreCuentas
    {
        [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
        public string NumeroCuentaOrigen { get; set; }

        [Required(ErrorMessage = "Debe ingresar la cuenta de destino")]
        public string NumeroCuentaDestino { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto que desea enviar")]

        [DataType(DataType.Text)]
        [Range(100, double.MaxValue, ErrorMessage = "Por favor digite un monto minimo de $100 DOP")]
        public double Monto { get; set; }

        public bool? HasError { get; set; }
        public string? Error { get; set; }
    }
}
