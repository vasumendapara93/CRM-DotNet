
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
            CreateMap<User, UserResponseDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();

            CreateMap<Branch, BranchCreateDTO>().ReverseMap();
            CreateMap<Branch, BranchUpdateDTO>().ReverseMap();
            CreateMap<Branch, BranchResponseDTO>().ReverseMap();

            CreateMap<Lead, LeadUpdateDTO>().ReverseMap();
            CreateMap<Lead, LeadCreateDTO>().ReverseMap();
            CreateMap<Lead, LeadResponseDTO>().ReverseMap();
        }
    }
}
