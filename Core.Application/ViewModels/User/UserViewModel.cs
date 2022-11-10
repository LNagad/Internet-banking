using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int? Status { get; set; }

        public string? ImagePath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string? ActivationKey { get; set; }

        public int? IdCard { get; set; }

        public int UserType { get; set; }

        //Navigation Properties
        public ICollection<Product>? Products { get; set; }
        public ICollection<Beneficiario>? Beneficiarios { get; set; }
    }
}
