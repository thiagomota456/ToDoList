using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;
using ToDoListAPI.Controllers.Base;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Category;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Model;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [Route("api/user")]
    public class UserController(IMapper mapper, AppDbContext context, IMemoryCache cache) : ToDoControllerBase(mapper, context, cache)
    {
        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Dados inválidos." });

            var checkUsername = _context.Users.FirstOrDefault(u => u.Username == dto.Username);

            if (checkUsername != null)
                return BadRequest(new { Error = "Username em uso." });

            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuario criada com sucesso!", Usuario = _mapper.Map<UserDto>(user) });
        }

        [Authorize]
        [HttpPost("Delete")]
        public IActionResult Delete()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == CurrentUserId());
            if (user == null) return NotFound();

            var token = CurrentTokenLogin();

            if (token == String.Empty)
                return Unauthorized(new { Error = "Token de autenticação não encontrado." });

            AuthService.RevokeJWTToken(_cache, token);

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuário excluído com sucesso!" });
        }

        [Authorize]
        [HttpGet("GetUserData/{complete?}")]
        public IActionResult GetUserData(bool complete = false)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == CurrentUserId());
            if (user == null) return NotFound();

            if (complete)
            {
                var userWithDetails = _context.Users
                    .Include(c => c.Categoria)
                    .Where(u => u.Id.ToString() == CurrentUserId())
                    .Select(u => new
                    {
                        u.Id,
                        u.Username,
                        Categories = u.Categoria.Select(c => new
                        {
                            c.Id,
                            c.Name,
                            Tasks = c.Tasks.Select(t => new
                            {
                                t.Id,
                                t.Title,
                                t.Description,
                                t.IsCompleted

                            }).ToList()

                        }).ToList()

                    }).FirstOrDefault();

                return Ok(userWithDetails);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("Update")]
        public IActionResult Update([FromBody] UserUpdateDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == CurrentUserId());
            if (user == null) return NotFound();

            _mapper.Map(dto, user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuario atualizado com sucesso!", Usuario = _mapper.Map<UserDto>(user) });
        }
    }
}
