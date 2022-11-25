using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Pagos.PagoAvance;

public class SavePagoAvanceViewModel
{
    [Required(ErrorMessage = "Debe ingresar el monto que desea pagar")]

    [DataType(DataType.Text)]
    public double Monto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
    public string NumeroCuentaOrigen { get; set; }
    
    public string NumeroTarjetaCredito { get; set; }

    public string? idTarjetaCredito { get; set; }

    public bool? HasError { get; set; }
    public string? Error { get; set; }
}