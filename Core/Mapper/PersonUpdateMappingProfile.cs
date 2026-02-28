using AutoMapper;
using ServiceContracts.DTO;
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
            CreateMap<PersonUpdateRequest, Person>();
        }
    }
}
