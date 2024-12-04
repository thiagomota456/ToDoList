using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Category;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [Route("api/user")]
    public class UserController : CrudControllerBase<UserDto, UserCreateDto, UserUpdateDto>
    {
        public UserController(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        [HttpPost("Create")]
        public override IActionResult Create([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Dados inválidos." });

            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuario criada com sucesso!", Usuario = _mapper.Map<UserDto>(user) });
        }

        [HttpPost("Delete/{id}")]
        public override IActionResult Delete(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { Message = "Usuário excluído com sucesso!" });
        }

        [HttpGet("GetAll")]
        public override IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        //Necessario alteração
        [HttpGet("GetById/{id}/{complete?}")]
        public override IActionResult GetById(string id, bool complete = false)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == id);
            if (user == null) return NotFound();

            if (complete)
            {
                var userWithDetails = _context.Users
                    .Where(u => u.Id.ToString() == id)
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

        [HttpPost("Update/{id}")]
        public override IActionResult Update(string id, [FromBody] UserUpdateDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == id);
            if (user == null) return NotFound();

            _mapper.Map(dto, user);
            _context.SaveChanges();

            return Ok(new { Message = "Usuario atualizado com sucesso!", Usuario = _mapper.Map<UserDto>(user) });
        }
    }
}
