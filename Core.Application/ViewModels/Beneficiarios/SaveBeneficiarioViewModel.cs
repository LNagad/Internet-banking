using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Beneficiarios
{
    public class SaveBeneficiarioViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe introducir el numero de cuenta" )]

        public string NumeroCuenta { get; set; }

        public string? IdCuentaAhorro { get; set; }

        public string? IdUser { get; set; }

        public string? IdBeneficiario { get; set; }

        public bool? HasError { get; set; }

        public string? Error { get; set; }
    }
}
