using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Model;

namespace ToDoListAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model.Task> Tasks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração de User -> Category (1:N)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User) // Cada categoria pertence a um User
                .WithMany(u => u.Categoria) // Um User tem muitas categorias
                .HasForeignKey(c => c.UserId) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Excluir categorias ao excluir o User

            // Configuração de Category -> Task (1:N)
            modelBuilder.Entity<Model.Task>()
                .HasOne(t => t.Category) // Cada tarefa pertence a uma categoria
                .WithMany(c => c.Tasks) // Uma categoria tem várias tarefas
                .HasForeignKey(t => t.CategoryId) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Excluir tarefas ao excluir a categoria

            // Configuração para User.Id (Preservar Guid gerado pelo código)
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever(); // Banco não gera o ID

            // Configuração para User.CreatedAt
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Apenas se o valor não for fornecido

            // Configuração para Task.Created e Task.UpdatedAt
            modelBuilder.Entity<Model.Task>()
                .Property(t => t.Created)
                .ValueGeneratedNever(); // Banco respeita o valor já gerado
            modelBuilder.Entity<Model.Task>()
                .Property(t => t.UpdatedAt)
                .ValueGeneratedNever(); // Banco respeita o valor já gerado

            base.OnModelCreating(modelBuilder);
        }
    }
}
