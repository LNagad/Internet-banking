using Core.Domain.Common;

namespace Core.Domain.Entities;

public class CuentaAhorro : AuditableBaseEntity
{
    public string NumeroCuenta { get; set; }
    
    public double Balance { get; set; }
    
    public int Principal { get; set; }
    
    //navigation property
    public int IdProduct { get; set; }
    public Product Product { get; set; }
    public ICollection<Beneficiario> Beneficiarios { get; set; }
}