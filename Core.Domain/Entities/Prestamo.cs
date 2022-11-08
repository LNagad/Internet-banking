using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Prestamo: AuditableBaseEntity
    {
        public string NumeroPrestamo { get; set; }
        
        public int Monto { get; set; }
        
        public int Pago { get; set; }
        
        public int Debe { get; set; } //1 , 0
        
        public int Balance { get; set; }
        
        //navigation property
        public int IdProduct { get; set; }
        public Product Product { get; set; }
    }
}
