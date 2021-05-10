using System.Collections.Generic;
using dotnetcore_rpg.Models;
using System.Linq;
using System.Threading.Tasks;
using dotnetcore_rpg.DTO;
using AutoMapper;
using System;
using dotnetcore_rpg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace dotnetcore_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }
        private int GetUserId()
        {
            int userId=int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(m=>m.Type==ClaimTypes.NameIdentifier).Value);
            return userId;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbData = await _context.Characters.Where(m => m.User.Id == GetUserId()).ToListAsync();
                response.Data = _mapper.Map<List<GetCharacterDto>>(dbData);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message + ex.Source + ex.StackTrace;
            }
            return response;
        }
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            Character data = await _context.Characters.FirstOrDefaultAsync(m => m.Id == id && m.User.Id==GetUserId());
            response.Data = _mapper.Map<GetCharacterDto>(data);
            return response;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            Character character = _mapper.Map<Character>(newCharacter);
            character.User=await _context.users.FirstOrDefaultAsync(m=>m.Id==GetUserId());
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            var data = await _context.Characters.Where(m=>m.User.Id==GetUserId()).ToListAsync();
            response.Data = _mapper.Map<List<GetCharacterDto>>(data);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto newCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.Include(m=>m.User).FirstOrDefaultAsync(m => m.Id == newCharacter.Id);
                if(character!=null && character.User.Id==GetUserId()){
                character.Name = newCharacter.Name;
                character.Strength = newCharacter.Strength;
                character.Class = newCharacter.Class;
                character.Defence = newCharacter.Defence;
                character.HitPoints = newCharacter.HitPoints;
                character.Intelligence = newCharacter.Intelligence;
                _context.Update(character);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
                }else{
                    response.Success=false;
                    response.Message="not able to update";
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                //Character character = await _context.Characters.FirstAsync(m => m.Id == id);
                Character character = await _context.Characters.FirstOrDefaultAsync(m => m.Id == id && m.User.Id==GetUserId());
                if(character!=null){
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                var dbData = await _context.Characters.Where(m=>m.User.Id==GetUserId()).ToListAsync();
                response.Data = _mapper.Map<List<GetCharacterDto>>(dbData);
                }else{
                    response.Success=false;
                    response.Message="Character not found";
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}