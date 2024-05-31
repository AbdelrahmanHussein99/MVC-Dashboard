using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Models
{
    public class Department :BaseEntity
    {
        public int Id { get; set; }


        public string Code { get; set; }

        public string Name { get; set; }


        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
