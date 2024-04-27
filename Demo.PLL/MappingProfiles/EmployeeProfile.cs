using AutoMapper;
using Demo.DAL.Models;
using Demo.PLL.ViewModels;

namespace Demo.PLL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }    
    }
}
