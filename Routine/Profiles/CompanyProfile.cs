using AutoMapper;
using Routine.Entity;
using Routine.Models;

namespace Routine.Profiles
{
    public class CompanyProfile:Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Name))
                .ForMember(
                dst=>dst.Id,
                opt=>opt.MapFrom(src=>src.Id));
            CreateMap< CompanyAddDto,Company>();
        }
    }
}
