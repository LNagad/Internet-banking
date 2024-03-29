﻿using Core.Application.ViewModels.CuentaAhorros;
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
        Task<CuentaAhorroViewModel> AccountExists(string NumeroCuenta);

        Task<SaveCuentaAhorroViewModel> Add(SaveCuentaAhorroViewModel vm, string userId, bool primary);

        Task<List<CuentaAhorroViewModel>> GetAllViewModelWithInclude(string user = null);

    }
}
