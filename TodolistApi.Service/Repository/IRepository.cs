
using TodolistApi.Domain.Models;

namespace TodolistApi.Service.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> Get();
        T? Get(int id);
        void Insert(T record);
        void Update(T record);
        void Delete(T record);
        Task SaveChangesAsync();
    }
}
