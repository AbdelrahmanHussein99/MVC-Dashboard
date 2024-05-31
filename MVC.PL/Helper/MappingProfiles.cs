using AutoMapper;
using MVC.DAL.Models;
using MVC.PL.ViewModels;

namespace MVC.PL.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
