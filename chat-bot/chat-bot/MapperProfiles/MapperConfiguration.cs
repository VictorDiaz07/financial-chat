using AutoMapper;
using Chat.Persistence.DTOs;
using Chat.Persistence.Entities;

namespace ChatAPI.MapperProfiles
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
