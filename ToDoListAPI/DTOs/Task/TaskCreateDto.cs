namespace ToDoListAPI.DTOs.Task
{
    public class TaskCreateDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required bool IsCompleted { get; set; } = false;
        public required int CategoryId { get; set; }
    }
}
