using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore_rpg.Models
{
    [Table("User")]
    public class User
    {
        public int Id{get;set;}
        public string UserName{get;set;}
        public byte[] PasswordHash{get;set;}
        public byte[] PasswordSalt{get;set;}
        public List<Character> characters{get;set;}
    }
}