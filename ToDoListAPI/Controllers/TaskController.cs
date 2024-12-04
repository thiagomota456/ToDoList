using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoListAPI.Data;
using ToDoListAPI.DTOs.Task;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [Route("api/Task")]
    public class TaskController : CrudControllerBase<DTOs.Task.TaskDto, DTOs.Task.TaskCreateDto, DTOs.Task.TaskUpdateDto>
    {
        public TaskController(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        [HttpPost("Create")]
        public override IActionResult Create([FromBody] TaskCreateDto dto)
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

        [HttpPost("Delete/{id}")]
        public override IActionResult Delete(string id)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada." });

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(new { Message = "Tarefa excluída com sucesso!" });
        }

        [HttpGet("GetAll")]
        public override IActionResult GetAll()
        {
            var tasks = _context.Tasks
            .Include(t => t.Category)
            .ToList();

            var taskDtos = _mapper.Map<List<TaskDto>>(tasks);

            return Ok(taskDtos);
        }

        [HttpGet("GetById/{id}")]
        public override IActionResult GetById(string id, bool complete = false)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada." });

            var taskDto = _mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [HttpPost("Update/{id}")]
        public override IActionResult Update(string id, [FromBody] TaskUpdateDto dto)
        {
            if (!int.TryParse(id, out var taskId))
                return BadRequest(new { Error = "ID inválido." });

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return NotFound(new { Error = "Tarefa não encontrada." });

            _mapper.Map(dto, task);

            _context.SaveChanges();

            return Ok(new { Message = "Tarefa atualizada com sucesso!", Task = _mapper.Map<TaskDto>(task) });
        }
    }
}
