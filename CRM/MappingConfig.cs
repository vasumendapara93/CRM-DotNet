
using AutoMapper;
using CRM.Models;
using CRM.Models.DTOs;

namespace CRM
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, RegisterationRequestDTO>().ReverseMap();
        }
    }
}
