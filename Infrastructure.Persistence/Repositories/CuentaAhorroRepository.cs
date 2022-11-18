using Core.Domain.Entities;
using Core.Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;

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
    }
}
