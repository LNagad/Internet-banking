using Core.Domain.Entities;
using Core.Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CuentaAhorroRepository : GenericRepository<CuentaAhorro>, ICuentaAhorroRepository
    {
        private readonly ApplicationContext _dbContext;

        public CuentaAhorroRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<CuentaAhorro> AddAsync(CuentaAhorro entity)
        {
            Guid guid = Guid.NewGuid();
            entity.Id = guid.ToString();

            return await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(CuentaAhorro entity, string id)
        {
            CuentaAhorro entry = await _dbContext.Set<CuentaAhorro>().FindAsync(id);

            entity.Created = entry.Created;
            entity.CreatedBy = entry.CreatedBy;

            await base.UpdateAsync(entity, id);
        }


        public async Task<CuentaAhorro> AccountExists(string NumeroCuenta)
        {
            CuentaAhorro cuenta = await _dbContext.Set<CuentaAhorro>()
                .FirstOrDefaultAsync(p => p.NumeroCuenta == NumeroCuenta);

            return cuenta;
        }
    }
}
