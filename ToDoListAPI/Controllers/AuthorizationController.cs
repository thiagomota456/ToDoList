using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListAPI.Controllers.Base;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController(IMapper mapper, AppDbContext context, IMemoryCache cache) : ToDoControllerBase(mapper, context, cache)
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {

            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);

            if (user == null || !Cryptography.ValidateHash(login.Password, user.PasswordHash))
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var tokenString = AuthService.GenerateTheJWTToken(user);

            return Ok(new { Token = tokenString });
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var token = CurrentTokenLogin();

            if (token == String.Empty)
                return Unauthorized(new { Error = "Token de autenticação não encontrado." });

            AuthService.RevokeJWTToken(_cache, token);

            return Ok(new { Message = "Logout realizado com sucesso!" });
        }

    }
}
