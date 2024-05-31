using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;

        private Lazy<IDepartmentRepository> _DepartmentRepository;
        private Lazy<IEmployeeRepository> _EmployeeRepository;



        public UnitOfWork(AppDbContext context)
        {
            _Context = context;
            _DepartmentRepository = new Lazy<IDepartmentRepository>(new DepartmentRepository(_Context));
            _EmployeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(_Context));
        }

        public IDepartmentRepository departmentRepository => _DepartmentRepository.Value;
        public IEmployeeRepository employeeRepository => _EmployeeRepository.Value;

        public async Task<int> Complete()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
