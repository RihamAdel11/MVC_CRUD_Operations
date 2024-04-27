using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
  
    {
        private readonly MVCAppContext _dbContext;

        public GenericRepositry(MVCAppContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            
        }

        public void Delete(T entity)
        {
           _dbContext.Remove(entity);
           
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee)) {
                return (IEnumerable<T>)await _dbContext.Employees .Include(E=>E.Departments).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
           
        }
    }
}
