using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [RegularExpression(@"(809|829|849)\d{3}\d{4}", ErrorMessage = "Tu numero de telefono es invalido (FORMATO RD)")]
        [Required]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        public int? Status { get; set; }

        public string? ImagePath { get; set; }
        public IFormFile? File { get; set; }

        [Required(ErrorMessage="Nombre de usuario incorrecto")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password incorrecta")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las password no coinciden")]
        [Required(ErrorMessage = "Necesitas llenar esta informacion")]
        public string SavePassword { get; set; }

        [Required(ErrorMessage = "Tienes que ingresar un correo electronico")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        public string? ActivationKey { get; set; }

        public int? IdCard { get; set; }

        public int? UserType { get; set; }
    }
}
