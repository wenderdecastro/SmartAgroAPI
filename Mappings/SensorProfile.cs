using AutoMapper;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Mappings
{
    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            CreateMap<Sensor, SensorDTO>().ReverseMap();
            CreateMap<Sensor, EditSensorDTO>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.UsuarioId, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LogsSensor, LogsSensorDTO>().ReverseMap();
        }
    }
}
