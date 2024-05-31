using Microsoft.EntityFrameworkCore;
using MVC.BLL.Interfaces;
using MVC.DAL.Data;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _Context;

        public GenericRepository(AppDbContext context)
        {
            _Context = context;
        }
        public async Task<IEnumerable<T>> GetAll()
        {

            if (typeof(T) == typeof(Employee))
            {

                return (IEnumerable<T>)await _Context.Employees.Include(E => E.Department).ToListAsync();
            }
            else
            {
                return await _Context.Set<T>().ToListAsync();

            }


        }


        public async Task<T> GetById(int id)
        {
            var result = await _Context.Set<T>().FindAsync(id);

            return result;
        }

        public void Add(T entity)
        {
            _Context.Add(entity);

        }


        public void Update(T entity)
        {
            _Context.Update(entity);

        }

        public void Delete(T entity)
        {
            _Context.Remove(entity);

        }
    }
}
