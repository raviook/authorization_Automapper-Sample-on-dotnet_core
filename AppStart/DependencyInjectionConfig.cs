using dotnetcore_rpg.Data;
using dotnetcore_rpg.Services.CharacterService;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetcore_rpg.AppStart
{
    public class DependencyInjectionConfig
    {
        public static void AddScope(IServiceCollection services)
        {
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IAuthRepository, AuthRepository>();           
        }       
    }
}