namespace ToDoListAPI.DTOs.Category
{
    public class CategoryWithDetails
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Model.Task>? Tasks { get; set; }
    }
}
