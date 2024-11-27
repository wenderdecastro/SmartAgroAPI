using AutoMapper;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, Usuario>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Usuario, UserDTO>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
