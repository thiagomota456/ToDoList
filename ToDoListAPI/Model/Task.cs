namespace ToDoListAPI.Model
{
    public class Task
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required bool IsCompleted { get; set; } = false;
        public required DateTime Created { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
