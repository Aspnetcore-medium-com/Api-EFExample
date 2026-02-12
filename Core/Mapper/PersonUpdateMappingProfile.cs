using AutoMapper;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class PersonUpdateMappingProfile : Profile
    {
        public PersonUpdateMappingProfile()
        {
            CreateMap<ServiceContracts.DTO.PersonUpdateRequest, Person>();
            CreateMap<Person, ServiceContracts.DTO.PersonResponse>();
        }
    }
}
