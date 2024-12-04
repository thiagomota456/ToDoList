namespace ToDoListAPI.Model
{
    public class Category
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Task>? Tasks { get; set; }
    }

}
