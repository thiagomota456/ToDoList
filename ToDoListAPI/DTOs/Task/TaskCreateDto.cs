namespace ToDoListAPI.DTOs.Task
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int CategoryId { get; set; }
    }
}
