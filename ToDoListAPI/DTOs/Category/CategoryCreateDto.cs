namespace ToDoListAPI.DTOs.Category
{
    public class CategoryCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
