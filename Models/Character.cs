using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore_rpg.Models
{
    [Table("Character")]
    public class Character
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public int HitPoints{get;set;}
        public int Strength{get;set;}
        public int Defence{get;set;}
        public int Intelligence{get;set;}
        public RpgClass Class{get;set;}
        public User User {get;set;}
    }
}