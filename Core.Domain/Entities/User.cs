using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Phone { get; set; }
        
        public int Status { get; set; } //1 = True, 0 = False
        
        public string ImagePath { get; set; }
        //public IFormFile File {get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public string ActivationKey { get; set; }
        
        public int IdCard { get; set; } 
        
        public int UserType { get; set; } //1 = True, 0 = False
        
        //Navigation Properties
        public ICollection<Product> Products { get; set; }
        public ICollection<Beneficiario>? Beneficiarios { get; set; }
    }
}
