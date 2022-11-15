using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Users
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Debe colocar el correo del usuario")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe tener un token")]
        [DataType(DataType.Text)]
        public string Token { get; set; }

        [Required(ErrorMessage = "Debe colocar una contrasena")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contrasenas no coinciden")]
        [Required(ErrorMessage = "Necesitas llenar esta informacion")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool? HasError { get; set; }

        public string? Error { get; set; }
    }
}
