using ToDoListAPI.Model;

namespace ToDoListAPI.DTOs.Task
{
    public class TaskDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required bool IsCompleted { get; set; }
        public required string CategoryName { get; set; }
    }
}
