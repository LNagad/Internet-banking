using Core.Domain.Common;

namespace Core.Domain.Entities;

public class Product : AuditableBaseEntity
{ 
    public int IdProduct { get; set; }
    
    public int IdProductType { get; set; }
    
    public int IdUser { get; set; }
    
    public int Primary { get; set; } // 1 = True, 0 = False
    
    //Navigation properties
    public User User { get; set; }
    public ICollection<CuentaAhorro> CuentaAhorros { get; set; }
    public ICollection<TarjetaCredito> TarjetaCreditos { get; set; }
    public ICollection<Prestamo> Prestamos { get; set; }
}