﻿using Core.Domain.Entities;

namespace Core.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> AccountExists(string productId);
    }
}
