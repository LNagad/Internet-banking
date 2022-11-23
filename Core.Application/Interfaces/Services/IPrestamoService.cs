using Core.Application.ViewModels.Prestamos;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.ViewModels.CuentaAhorros;

namespace Core.Application.Interfaces.Services
{
    public interface IPrestamoService : IGenericService<SavePrestamoViewModel, PrestamoViewModel ,Prestamo>
    {
        Task<SavePrestamoViewModel> AddCuentaAhorro(SavePrestamoViewModel prestamoVm, string userId); 
    }
}
