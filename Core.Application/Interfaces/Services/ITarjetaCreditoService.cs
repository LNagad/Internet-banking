﻿using Core.Application.ViewModels.TarjetaCreditos;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface ITarjetaCreditoService : IGenericService<SaveTarjetaCreditoViewModel, TarjetaCreditoViewModel, TarjetaCredito>
    {
        Task<SaveTarjetaCreditoViewModel> AddTarjetaCredito(SaveTarjetaCreditoViewModel vm);

        Task<List<TarjetaCreditoViewModel>> GetAllTarjetaById(string id);
    }
}
