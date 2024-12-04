namespace ToDoListAPI.DTOs.User
{
    public class UserDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
