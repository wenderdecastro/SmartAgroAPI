using AutoMapper;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Usuario, Usuario>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.Senha, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserEditDTO, Usuario>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.Senha, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserEditDTO, Usuario>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.Senha, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
