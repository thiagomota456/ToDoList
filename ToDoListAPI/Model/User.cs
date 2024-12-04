namespace ToDoListAPI.Model
{
    public class User
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Category>? Categoria { get; set; }
    }
}
