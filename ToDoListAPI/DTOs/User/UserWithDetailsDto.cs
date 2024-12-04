namespace ToDoListAPI.DTOs.User
{
    public class UserWithDetailsDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public ICollection<Model.Category>? Categoria { get; set; }
    }
}
