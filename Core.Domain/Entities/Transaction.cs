using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class Transaction : AuditableBaseEntity
    {
        public string UserId { get; set; }

        public string FromId { get; set; }

        public string ProductFromId { get; set; }
        
        public string ToId { get; set; }

        public string ProductToId { get; set; }

        public bool? isCuentaAhorro { get; set; }
        public bool? isTarjetaCredito { get; set; }
        public bool? isPrestamo { get; set; }
    }
}
