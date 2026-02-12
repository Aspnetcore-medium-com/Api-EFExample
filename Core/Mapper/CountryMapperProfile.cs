using AutoMapper;
using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class CountryMapperProfile : Profile
    {
        public CountryMapperProfile()
        {
            CreateMap<CountryAddRequest, Country>()
                .ForMember(dest=> dest.CountryId, opt => opt.Ignore());

            CreateMap<Country, CountryResponse>();
        }
    }
}
