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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Product> AddAsync(Product entity)
        {
            Guid guid = Guid.NewGuid();
            entity.Id = guid.ToString();

            return await base.AddAsync(entity);
        }

        public virtual async Task<List<Product>> GetAllWithInclude(List<string> properties)
        {
            var query = _dbContext.Set<Product>().AsQueryable();
            List<Product> lista = new();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }
            try
            {
                 lista = await query.ToListAsync();

            } catch(Exception ex)
            {

            }

            return lista;
        }

    }
}
