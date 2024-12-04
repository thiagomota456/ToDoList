namespace ToDoListAPI.DTOs.Category
{
    public class CategoryUpdate
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
