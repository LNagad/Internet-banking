using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Common
{
    public class AuditableBaseEntity
    {
        public string Id { get; set; }
        
        public DateTime? Created { get; set; }
        
        public string CreatedBy { get; set; } //Id del user/admin que lo creo
        
        public DateTime? LastModified { get; set; }
        
        public string? LastModifiedBy { get; set; }
    }
}
