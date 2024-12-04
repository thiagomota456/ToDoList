namespace ToDoListAPI.DTOs.User
{
    public class UserCreateDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

    }
}
