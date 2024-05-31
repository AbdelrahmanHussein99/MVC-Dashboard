using Microsoft.AspNetCore.Http;
using MVC.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace MVC.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required ")]
        public string Name { get; set; }
        [Range(22, 45)]
        public int? Age { get; set; }

        public decimal Salary { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Is Active")]

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        [DisplayName("Hire Date")]

        public DateTime HireDate { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }


        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
