using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetcore_rpg.DTO;
using dotnetcore_rpg.Models;

namespace dotnetcore_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
       Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
       Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
       Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
       Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto newCharacter);
       Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    }

}