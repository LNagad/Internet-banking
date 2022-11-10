using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public int IdProductType { get; set; }

        public int IdUser { get; set; }

        public int Primary { get; set; } // 1 = True, 0 = False

    }
}
