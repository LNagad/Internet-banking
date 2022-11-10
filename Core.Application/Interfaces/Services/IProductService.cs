using Core.Application.ViewModels.Prestamos;
using Core.Application.ViewModels.Products;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel ,Product>
    {
    }
}
