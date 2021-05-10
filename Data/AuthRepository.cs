using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnetcore_rpg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnetcore_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;

        }
        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.users.FirstOrDefaultAsync(m => m.UserName.ToLower() == userName.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Data = "User Not Found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash))
            {
                response.Success = false;
                response.Data = "Password mismatch";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            try
            {
                if (await UsersExists(user.UserName))
                {
                    response.Success = false;
                    response.Message = "User Already Exists";
                    return response;
                }
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
                response.Data = user.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<bool> UsersExists(string userName)
        {

            if (await _context.users.AnyAsync(m => m.UserName.ToLower() == userName.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (passwordHash[i] != computeHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}