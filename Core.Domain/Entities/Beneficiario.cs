using Core.Domain.Common;

namespace Core.Domain.Entities;

public class Beneficiario : AuditableBaseEntity
{
    public string IdAccount { get; set; }
    
    public string IdUser { get; set; }

    public string IdBeneficiario { get; set; }

    //Navigation property
    public CuentaAhorro CuentaAhorro { get; set; }
}