namespace Core.Application.Dtos.Pagos;

public class PagoAvanceEfectivoResponse
{
    public string FirstNameOrigen { get; set; }

    public string NumeroCuenta { get; set; }

    public string LastNameOrigen { get; set; }

    public string NumeroTarjeta { get; set; }

    public double Monto { get; set; }
    
    public double MontoCargado { get; set; }
    
    public double oldMontoCuenta { get; set; }
    
    public double newMontoCuenta { get; set; }

    public bool HasError { get; set; }
    public string? Error { get; set; }
}