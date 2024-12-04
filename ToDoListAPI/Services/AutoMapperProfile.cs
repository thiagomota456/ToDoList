using AutoMapper;
using ToDoListAPI.DTOs;
using ToDoListAPI.DTOs.Category;
using ToDoListAPI.DTOs.Task;
using ToDoListAPI.DTOs.User;
using ToDoListAPI.Model;

namespace ToDoListAPI.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            //User
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Id,           opt => opt.MapFrom( _  => Guid.NewGuid()))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => Cryptography.GenerateHash(src.Password)))
                .ForMember(dest => dest.CreatedAt,    opt => opt.MapFrom( _  => DateTime.UtcNow));

            CreateMap<UserUpdateDto, User>()
                  .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => Cryptography.GenerateHash(src.Password)));

            CreateMap<User, UserDto>().ReverseMap();

            //Category
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryUpdate, Category>().ReverseMap();


            //Task
            CreateMap<TaskCreateDto, Model.Task>()
                 .ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow))
                 .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<TaskUpdateDto, Model.Task>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Model.Task, TaskDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Sem Categoria"));

        }
    }
}
