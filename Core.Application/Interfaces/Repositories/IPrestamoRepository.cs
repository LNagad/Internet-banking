using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repositories
{
    public interface IPrestamoRepository : IGenericRepository<Prestamo>
    {
        Task<Prestamo> PrestamoExist(string productId);
    }
}
