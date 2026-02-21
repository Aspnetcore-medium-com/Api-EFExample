using AutoMapper;
using Microsoft.Extensions.Logging;
using ServiceContracts.DTO;
using ServiceContracts.enums;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {

            CreateMap<ServiceContracts.DTO.PersonAddRequest, Person>()
                .ForMember(dest => dest.PersonId, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null));

            CreateMap<Person, ServiceContracts.DTO.PersonResponse>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Gender) ? (GenderOptions?)null : Enum.Parse<GenderOptions>(src.Gender)))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.CountryName : null));


        }
    }
}
