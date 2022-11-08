using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Common
{
    public class AuditableBaseEntity
    {
        public int Id { get; set; }
        
        public DateTime? Created { get; set; }
        
        public int CreateBy { get; set; } //Id del user/admin que lo creo
        
        public DateTime? LastModified { get; set; }
        
        public DateTime? LastModifiedBy { get; set; }
    }
}
