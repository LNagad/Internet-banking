using Core.Domain.Common;

namespace Core.Domain.Entities;

public class TarjetaCredito : AuditableBaseEntity
{
    public string NumeroTarjeta { get; set; }
    
    public int Limite { get; set; }
    
    public int Pago { get; set; }
    
    public int Debe { get; set; } // 1, 0
    
    public int Balance { get; set; }
    
    //navigation property
    public int Idproduct { get; set; }
    public Product Product { get; set; }
}