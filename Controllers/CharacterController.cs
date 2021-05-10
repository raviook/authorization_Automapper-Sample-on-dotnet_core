using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using dotnetcore_rpg.Models;
using System.Linq;
using dotnetcore_rpg.Services.CharacterService;
using System.Threading.Tasks;
using dotnetcore_rpg.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dotnetcore_rpg.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {

        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response=await _characterService.UpdateCharacter(updateCharacter);
            if(response.Data==null){
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id){
            var response=await _characterService.DeleteCharacter(id);
            if(response.Data==null){
                return NotFound(response);
            }
            return Ok(response);
        }
    }


}