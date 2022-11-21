using Core.Domain.Common;

namespace Core.Domain.Entities;

public class Beneficiario : AuditableBaseEntity
{
    public string NumeroCuenta { get; set; }
    
    public string IdUser { get; set; }

    public string IdBeneficiario { get; set; }


    //Navigation property
    public string IdCuentaAhorro { get; set; }
    public CuentaAhorro CuentaAhorro { get; set; }
}