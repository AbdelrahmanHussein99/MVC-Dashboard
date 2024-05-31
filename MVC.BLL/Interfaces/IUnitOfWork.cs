using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IDepartmentRepository departmentRepository { get; }
        public IEmployeeRepository employeeRepository { get; }

        public Task<int> Complete();
    }
}
