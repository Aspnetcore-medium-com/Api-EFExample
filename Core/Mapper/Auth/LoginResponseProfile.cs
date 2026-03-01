using AutoMapper;
using Core.Domain.IdentityEntities;
using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper.Auth
{
    public class LoginResponseProfile: Profile
    {
        public LoginResponseProfile()
        {
            CreateMap<ApplicationUser, SignInResponse>()
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName != null ? src.PersonName : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email : null));
        }
    }
}
