using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using ToDoListAPI.Controllers.Base;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Category;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [Route("api/category")]
    public class CategoryController(IMapper mapper, AppDbContext context, IMemoryCache cache) : ToDoControllerBase(mapper, context, cache)
    {
        [Authorize]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Dados inválidos." });

            var category = _mapper.Map<Category>(dto);
            category.UserId = new Guid(CurrentUserId());

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria criada com sucesso!", Category = _mapper.Map<CategoryDto>(category) });
        }

        [Authorize]
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId && c.UserId.ToString() == CurrentUserId());

            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada para o usuario atual" });

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria excluída com sucesso!" });
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.Where( c => c.UserId.ToString() == CurrentUserId()).ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [Authorize]
        [HttpGet("GetById/{id}/{complete?}")]
        public IActionResult GetById(string id, bool complete = false)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var currentUserId = CurrentUserId();

            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            var category = _context.Categories
                            .Include(t => t.Tasks)
                            .FirstOrDefault(c => c.Id == categoryId && c.UserId.ToString() == currentUserId);

            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada." });

            if (complete)
            {
                var categoryWithTasks = new
                {
                    category.Id,
                    category.Name,
                    category.Description,
                    Tasks = category.Tasks!.Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.Description,
                        t.IsCompleted

                    }).ToList()

                };

                return Ok(categoryWithTasks);
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [Authorize]
        [HttpPost("Update/{id}")]
        public IActionResult Update(string id, [FromBody] CategoryUpdate dto)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId && c.UserId.ToString() == CurrentUserId());
            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada." });

            _mapper.Map(dto, category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria atualizada com sucesso!", Category = _mapper.Map<CategoryDto>(category) });
        }
    }
}
