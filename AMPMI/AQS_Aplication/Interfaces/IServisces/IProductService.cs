﻿using AQS_Common.Enums;
using Domin.Entities;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface IProductService
    {
        Task<long> Create(Product product);
        Task<ResultServiceMethods> Delete(long id);
        Task<List<Product>> Read();
        Task<Product?> ReadById(long id);
        Task<List<Product>> ReadByCategoryId(int categoryId);
        Task<ResultServiceMethods> Update(Product product);
        Task<ResultServiceMethods> IsConfirmed(long id, bool isConfirmed);
        Task<ResultServiceMethods> UpdatePictureFileName(int id, Guid pictureFileName);
    }
}
