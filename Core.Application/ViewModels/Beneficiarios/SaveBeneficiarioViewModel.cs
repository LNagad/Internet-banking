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
        public string Id { get; set; }

        public string IdAccount { get; set; }

        public string IdUser { get; set; }

        public string IdBeneficiario { get; set; }
    }
}
