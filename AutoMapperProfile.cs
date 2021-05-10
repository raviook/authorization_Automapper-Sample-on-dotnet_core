using AutoMapper;
using dotnetcore_rpg.DTO;
using dotnetcore_rpg.Models;

namespace dotnetcore_rpg
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto,Character>();
        }
    }
}