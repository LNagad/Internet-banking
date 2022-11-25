using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string FromId { get; set; }

        public string ProductFromId { get; set; }

        public string ToId { get; set; }

        public string ProductToId { get; set; }

        public bool? isCuentaAhorro { get; set; }
        public bool? isTarjetaCredito { get; set; }
        public bool? isPrestamo { get; set; }

        public DateTime? Created { get; set; }
    }
}
