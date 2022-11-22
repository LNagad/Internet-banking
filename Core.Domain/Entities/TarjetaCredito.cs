using Core.Domain.Common;

namespace Core.Domain.Entities;

public class TarjetaCredito : AuditableBaseEntity
{
    public string NumeroTarjeta { get; set; }
    
    public double Limite { get; set; }
    
    public double Pago { get; set; }
    
    public double Debe { get; set; } // 1, 0
    
    public double Monto { get; set; }

    //navigation property
    public string IdProduct { get; set; }
    public Product Product { get; set; }
}