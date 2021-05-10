using dotnetcore_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore_rpg.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Character> Characters {get;set;}
        public DbSet<User> users{get;set;}
        public DbSet<Demo> demos{get;set;} 
    }
}