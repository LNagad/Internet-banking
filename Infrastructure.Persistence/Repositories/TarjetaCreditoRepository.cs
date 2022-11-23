using Core.Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TarjetaCreditoRepository : GenericRepository<TarjetaCredito>, ITarjetaCreditoRepository
    {
        private readonly ApplicationContext _dbContext;

        public TarjetaCreditoRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<TarjetaCredito> AddAsync(TarjetaCredito entity)
        {
            Guid guid = Guid.NewGuid();
            entity.Id = guid.ToString();

            return await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(TarjetaCredito entity, string id)
        {
            TarjetaCredito entry = await _dbContext.Set<TarjetaCredito>().FindAsync(id);

            entity.Created = entry.Created;
            entity.CreatedBy = entry.CreatedBy;
            entity.IdProduct= entry.IdProduct;
            entity.Limite = entry.Limite;
            entity.NumeroTarjeta= entry.NumeroTarjeta;


            await base.UpdateAsync(entity, id);
        }

        public async Task<TarjetaCredito> TarjetaExist(string productId)
        {
            TarjetaCredito tarjeta = await _dbContext.Set<TarjetaCredito>()
                .FirstOrDefaultAsync(p => p.IdProduct == productId);

            return tarjeta;
        }
    }
}
