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
    }
}
