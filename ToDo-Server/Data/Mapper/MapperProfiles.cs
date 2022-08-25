using AutoMapper;

using ToDo_Server.Data.Models;

namespace ToDo_Server.Data.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<ToDoReadDto, ToDo>().ReverseMap();
            CreateMap<ToDoUpdateDto, ToDo>().ReverseMap();

            CreateMap<AppRegisterDto, AppUser>().ReverseMap();
            CreateMap<UserDto, AppUser>().ReverseMap();

        }
    }
}
