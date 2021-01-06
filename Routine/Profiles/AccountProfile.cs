using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Routine.Models;

namespace Routine.Profiles
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterDto, IdentityUser>()
                .ForMember(dest=>dest.UserName,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dst=>dst.Email,
                    opt=>opt.MapFrom(src=>src.Email));
        }
    }
}
