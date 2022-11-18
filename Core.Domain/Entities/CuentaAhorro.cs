using Core.Domain.Common;

namespace Core.Domain.Entities;

public class CuentaAhorro : AuditableBaseEntity
{
    public string NumeroCuenta { get; set; }
    
    public double Balance { get; set; }

    public bool Principal { get; set; }

    //navigation property
    public string? IdProduct { get; set; }
    public Product Product { get; set; }
    public ICollection<Beneficiario> Beneficiarios { get; set; }
}