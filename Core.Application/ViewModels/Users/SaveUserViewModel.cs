using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {

        [Required(ErrorMessage = "Debe colocar el nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar el apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [RegularExpression(@"(809|829|849)\d{3}\d{4}", ErrorMessage = "Tu numero de telefono es invalido (FORMATO RD)")]
        [Required(ErrorMessage = "Debe colocar el numero de telefono")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }
        
        [RegularExpression(@"\d{11}", ErrorMessage = "Numero de cedula invalido (FORMATO RD)" )]
        [Required]
        public string Cedula { get; set; }
        
        public double? Monto { get; set; }

        [Required(ErrorMessage= "Debe colocar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Tienes que ingresar un correo electronico")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar una contrasena")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Necesitas llenar esta informacion")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contrasenas no coinciden")]
        public string ConfirmPassword { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
        public int UserType { get; set; }
    }
}
