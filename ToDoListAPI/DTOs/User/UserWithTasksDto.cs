namespace ToDoListAPI.DTOs.User
{
    public class UserWithTasksDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public ICollection<Model.Task>? Tarefas { get; set; }
    }
}
