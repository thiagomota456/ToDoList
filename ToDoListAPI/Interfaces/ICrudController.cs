using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListAPI.Interfaces
{
    public interface ICrudController<TDto, TCreateDto, TUpdateDto>
    {
        IActionResult Create([FromBody] TCreateDto dto);
        IActionResult Update(string id, [FromBody] TUpdateDto dto);
        IActionResult Delete(string id);
        IActionResult GetAll();
        IActionResult GetById(string id, bool complete = false);
    }
}
