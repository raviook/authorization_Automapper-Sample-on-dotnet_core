using dotnetcore_rpg.Models;

namespace dotnetcore_rpg.DTO
{
    public class AddCharacterDto
    {
        public string Name {get;set;}
        public int HitPoints{get;set;}
        public int Strength{get;set;}
        public int Defence{get;set;}
        public int Intelligence{get;set;}
        public RpgClass Class{get;set;}
    }
}