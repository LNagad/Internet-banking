using Core.Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PrestamoRepository : GenericRepository<Prestamo>, IPrestamoRepository
    {
        private readonly ApplicationContext _dbContext;

        public PrestamoRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Prestamo> AddAsync(Prestamo entity)
        {
            Guid guid = Guid.NewGuid();
            entity.Id = guid.ToString();

            return await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Prestamo entity, string id)
        {
            Prestamo entry = await _dbContext.Set<Prestamo>().FindAsync(id);

            entity.Created = entry.Created;
            entity.CreatedBy = entry.CreatedBy;
            entity.IdProduct = entry.IdProduct;
            entity.Monto = entry.Monto;
            entity.IdProduct = entry.IdProduct;
            entity.NumeroPrestamo = entry.NumeroPrestamo;

            await base.UpdateAsync(entity, id);
        }

        public async Task<Prestamo> PrestamoExist(string productId)
        {
            Prestamo tarjeta = await _dbContext.Set<Prestamo>()
                .FirstOrDefaultAsync(p => p.IdProduct == productId);

            return tarjeta;
        }
    }
}
