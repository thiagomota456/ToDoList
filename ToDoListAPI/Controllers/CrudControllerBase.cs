using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Data;
using ToDoListAPI.Interfaces;
using ToDoListAPI.Model;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    public abstract class CrudControllerBase<TDto, TCreateDto, TUpdateDto> : ControllerBase, ICrudController<TDto, TCreateDto, TUpdateDto>
    {
        protected IMapper _mapper;

        protected readonly AppDbContext _context;

        public CrudControllerBase(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public abstract IActionResult Create([FromBody] TCreateDto dto);
        public abstract IActionResult Update(string id, [FromBody] TUpdateDto dto);
        public abstract IActionResult Delete(string id);
        public abstract IActionResult GetAll();
        public abstract IActionResult GetById(string id, bool complete = false);
    }
}
