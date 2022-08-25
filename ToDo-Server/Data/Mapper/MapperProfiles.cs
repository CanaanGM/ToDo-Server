using AutoMapper;

using ToDo_Server.Data.Models;

namespace ToDo_Server.Data.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<ToDoDTO, ToDo>().ReverseMap();
        }
    }
}
