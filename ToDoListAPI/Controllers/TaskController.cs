using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoListAPI.Controllers.Base;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Task;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [Route("api/Task")]
    public class TaskController(IMapper mapper, AppDbContext context, IMemoryCache cache) : ToDoControllerBase(mapper, context, cache)
    {
        [Authorize]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] TaskCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { Error = "Dados inválidos." });

            var task = _mapper.Map<Model.Task>(dto);

            _context.Tasks.Add(task);
            _context.SaveChanges();

            var returnTask = _mapper.Map<TaskDto>(task);

            returnTask.CategoryName = _context.Categories
                    .Where(c => c.Id == task.CategoryId)
                    .Select(c => c.Name)
                    .FirstOrDefault()!;

            return Ok(new { Message = "Tarefa criada com sucesso!", Task = returnTask });
        }

        [Authorize]
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            // Obter o ID do usuário logado
            var userId = CurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Error = "Usuário não autenticado." });

            // Filtrar a tarefa pelo ID da tarefa e pelo usuário logado
            var task = _context.Tasks
                .Include(t => t.Category) // Inclui a categoria para acessar o UserId
                .FirstOrDefault(t => t.Id == taskId && t.Category != null && t.Category.UserId.ToString() == userId);

            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada ou não pertence ao usuário logado." });

            // Remove a tarefa
            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(new { Message = "Tarefa excluída com sucesso!" });
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var tasks = _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.Category != null && t.Category.UserId.ToString() == CurrentUserId())
                .ToList();

            var taskDtos = _mapper.Map<List<TaskDto>>(tasks);

            return Ok(taskDtos);
        }

        [Authorize]
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            var task = _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.Category != null && t.Category.UserId.ToString() == CurrentUserId())
                .FirstOrDefault(t => t.Id == taskId);

            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada." });

            var taskDto = _mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [Authorize]
        [HttpPost("Update/{id}")]
        public IActionResult Update(string id, [FromBody] TaskUpdateDto dto)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            var task = _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.Category != null && t.Category.UserId.ToString() == CurrentUserId())
                .FirstOrDefault(t => t.Id == taskId);


            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada." });

            _mapper.Map(dto, task);

            _context.SaveChanges();

            return Ok(new { Message = "Tarefa atualizada com sucesso!", Task = _mapper.Map<TaskDto>(task) });
        }
    }
}
