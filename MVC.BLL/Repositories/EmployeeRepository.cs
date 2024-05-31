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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Employee>> GetByName(string name)
        {
            return await _Context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department).ToListAsync();
        }
    }
}
