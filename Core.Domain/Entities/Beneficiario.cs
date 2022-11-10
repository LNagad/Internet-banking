using Core.Domain.Common;

namespace Core.Domain.Entities;

public class Beneficiario : AuditableBaseEntity
{
    public int IdAccount { get; set; }
    
    public int IdUser { get; set; }

    public int IdBeneficiario { get; set; }

    //Navigation property
    public CuentaAhorro CuentaAhorro { get; set; }
}