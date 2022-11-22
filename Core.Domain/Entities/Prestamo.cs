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
        
        public double Monto { get; set; }
        
        public double Pago { get; set; }
        
        public double Debe { get; set; } //1 , 0

        //navigation property
        public string IdProduct { get; set; }
        public Product Product { get; set; }
    }
}
