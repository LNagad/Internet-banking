namespace Core.Domain.Entities;

public class Beneficiario
{
    public int Id { get; set; }
    
    public int IdAccount { get; set; }
    
    public int IdUser { get; set; }

    public int IdBeneficiario { get; set; }

    //Navigation property
    public User user { get; set; }
    public CuentaAhorro CuentaAhorro { get; set; }
}