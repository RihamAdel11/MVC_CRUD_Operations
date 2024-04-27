using AutoMapper;
using Demo.DAL.Models;
using Demo.PLL.ViewModels;

namespace Demo.PLL.MappingProfiles
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department >().ReverseMap();
        }
    }
}
