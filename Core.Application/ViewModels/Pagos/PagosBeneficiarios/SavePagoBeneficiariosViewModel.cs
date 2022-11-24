using System.ComponentModel.DataAnnotations;

namespace Core.Application.ViewModels.Pagos.PagosBeneficiarios;

public class SavePagoBeneficiariosViewModel
{
    [Required(ErrorMessage = "Debe ingresar el monto que desea pagar")]

    [DataType(DataType.Text)]
    public double Monto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la cuenta de origen")]
    public string NumeroCuentaOrigen { get; set; }
    
    public string NumeroCuentaDestino { get; set; }

    public string? idProduct { get; set; }
    
    public string? BeneficiarioName { get; set; }
    
    public string? BeneficiarioLastName { get; set; }

    public bool? HasError { get; set; }
    public string? Error { get; set; }
}