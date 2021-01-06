using AutoMapper;
using Routine.Entity;
using Routine.Models;
using System;

namespace Routine.Profiles
{
    public class EmployProfile:Profile
    {
        public EmployProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dst => dst.Name,
                opt => opt.MapFrom(src => $"{src.LastName}{src.FirstName}"))
                .ForMember(dst => dst.GenderDisplay,
                opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dst => dst.Age,
                opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year));
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<Employee, EmployeeAddDto>();
        }
    }
}
