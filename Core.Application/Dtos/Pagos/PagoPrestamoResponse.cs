using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Dtos.Pagos
{
    public class PagoPrestamoResponse
    {
        public string FirstNameOrigen { get; set; }

        public string LastNameOrigen { get; set; }

        public string NumeroCuentaOrigen { get; set; }

        public string? NumeroPrestamo { get; set; }

        public double Monto { get; set; }

        public string? TransactionId { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
