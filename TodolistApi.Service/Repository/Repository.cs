using Microsoft.EntityFrameworkCore;
using TodolistApi.Domain.Data;
using TodolistApi.Domain.Models;

namespace TodolistApi.Service.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        
        
        public Repository(ApplicationDbContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
            _dbSet = _context.Set<T>(); 
        }

        public IQueryable<T> Get() =>
            _dbSet;

        public T? Get(int id) => _dbSet
            .Where(record => record.Id == id)
            .ToArray()
            .SingleOrDefault();
        public T? Get(int id,string userId) => _dbSet
            .Where(record => record.Id == id && userId == record.UserID)
            .ToArray()
            .SingleOrDefault();
        public T[] Get(string userId) => _dbSet
            .Where(record => userId == record.UserID)
            .ToArray();
            
            

        public void Insert(T record)
        {
            ArgumentNullException.ThrowIfNull(record);   
            _dbSet.Add(record);
        }

        public void Update(T record)
        {
            ArgumentNullException.ThrowIfNull(record);
            _dbSet.Update(record);
        }
       
        public void Delete(T record)
        {
            ArgumentNullException.ThrowIfNull(record);
            _dbSet.Remove(record);
        }

        public Task SaveChangesAsync() => 
            _context.SaveChangesAsync();
    }
}
