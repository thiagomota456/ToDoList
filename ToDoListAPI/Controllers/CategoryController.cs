using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Category;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [Route("api/category")]
    public class CategoryController : CrudControllerBase<CategoryDto, CategoryCreateDto, CategoryUpdate>
    {
        public CategoryController(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        [HttpPost("Create")]
        public override IActionResult Create([FromBody] CategoryCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Dados inválidos." });

            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria criada com sucesso!", Category = _mapper.Map<CategoryDto>(category) });
        }

        [HttpPost("Delete/{id}")]
        public override IActionResult Delete(string id)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada." });

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria excluída com sucesso!" });
        }

        [HttpGet("GetAll")]
        public override IActionResult GetAll()
        {
            var categories = _context.Categories.ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [HttpGet("GetById/{id}/{complete?}")]
        public override IActionResult GetById(string id, bool complete = false)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada." });

            if (complete)
            {
                var categoryWithTasks = _context.Categories
                    .Where(c => c.Id == categoryId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Description,
                        Tasks = c.Tasks.Select(t => new
                        {
                            t.Id,
                            t.Title,
                            t.Description,
                            t.IsCompleted

                        }).ToList()

                    }).FirstOrDefault();

                return Ok(categoryWithTasks);
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost("Update/{id}")]
        public override IActionResult Update(string id, [FromBody] CategoryUpdate dto)
        {
            if (!int.TryParse(id, out var categoryId))
                return BadRequest(new { Error = "ID inválido." });

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return NotFound(new { Error = "Categoria não encontrada." });

            _mapper.Map(dto, category);
            _context.SaveChanges();

            return Ok(new { Message = "Categoria atualizada com sucesso!", Category = _mapper.Map<CategoryDto>(category) });
        }
    }
}
