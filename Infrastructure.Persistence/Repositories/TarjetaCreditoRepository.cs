using Core.Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Infrastructure.Persistence.Contexts;
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
    }
}
