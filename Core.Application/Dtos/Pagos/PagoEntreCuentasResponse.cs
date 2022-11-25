
namespace Core.Application.Dtos.Pagos
{
    public class PagoEntreCuentasResponse
    {
        public string FirstNameOrigen { get; set; }

        public string LastNameOrigen { get; set; }

        public string NumeroCuentaOrigen { get; set; }


        public string FirstNameDestino { get; set; }

        public string LastNameDestino { get; set; }
        public string NumeroCuentaDestino { get; set; }

        public double Monto { get; set; }

        public string? TransactionId { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
