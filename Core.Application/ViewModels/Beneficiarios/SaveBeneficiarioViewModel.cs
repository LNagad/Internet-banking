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
        public int Id { get; set; }

        public int IdAccount { get; set; }

        public int IdUser { get; set; }

        public int IdBeneficiario { get; set; }
    }
}
