using Core.Application.ViewModels.CuentaAhorros;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface ICuentaAhorroService : IGenericService<SaveCuentaAhorroViewModel, CuentaAhorroViewModel ,CuentaAhorro>
    {

    }
}
