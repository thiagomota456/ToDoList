using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using ToDoListAPI.Data;

namespace ToDoListAPI.Controllers.Base
{
    [ApiController]
    public abstract class ToDoControllerBase : ControllerBase
    {
        protected IMapper _mapper;
        protected readonly AppDbContext _context;
        protected  readonly IMemoryCache _cache;
        public ToDoControllerBase(IMapper mapper, AppDbContext context, IMemoryCache cache)
        {
            _mapper = mapper;
            _context = context;
            _cache = cache;
        }

        [NonAction]
        public string CurrentUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        }

        [NonAction]
        public string CurrentTokenLogin() 
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return String.Empty;
            }

            return authHeader.Substring("Bearer ".Length).Trim();
        }
    }
}
