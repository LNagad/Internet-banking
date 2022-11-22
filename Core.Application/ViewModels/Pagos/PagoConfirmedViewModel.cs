using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Pagos
{
    public class PagoConfirmedViewModel
    {
        #region "Cuenta ahorro - pago expreso properties"
        public string FirstNameOrigen { get; set; }

        public string LastNameOrigen { get; set; }

        public string NumeroCuentaOrigen { get; set; }


        public string FirstNameDestino { get; set; }

        public string LastNameDestino { get; set; }
        public string NumeroCuentaDestino { get; set; }

        #endregion

        #region "Transaccion properties"

        public double Monto { get; set; }

        public string? TransactionId { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

        #endregion

        #region "Prestamo pago o tarjeta pago"

        public string? NumeroPrestamo { get; set; }

        public string? LastTarjetaCredito { get; set; }

        #endregion

        public bool? isCuentaAhorro { get; set; }
        public bool? isTarjetaCredito { get; set; }
        public bool? isPrestamo { get; set; }
    }
}
