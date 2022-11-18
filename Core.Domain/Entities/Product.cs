using Core.Domain.Common;

namespace Core.Domain.Entities;

public class Product : AuditableBaseEntity
{ 
    
    public string? IdProductType { get; set; }

    public bool? isCuentaAhorro { get; set; }
    public bool? isTarjetaCredito { get; set; }
    public bool? isPrestamo { get; set; }

    public string? IdUser { get; set; }
    
    public bool? Primary { get; set; } // 1 = True, 0 = False
    
    //Navigation properties
    public ICollection<CuentaAhorro>? CuentaAhorros { get; set; }
    public ICollection<TarjetaCredito>? TarjetaCreditos { get; set; }
    public ICollection<Prestamo>? Prestamos { get; set; }
}