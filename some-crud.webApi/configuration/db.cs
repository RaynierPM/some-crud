using Microsoft.EntityFrameworkCore;
using someCrud.DI.models;

namespace someCrud.configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<NoteEntity> Notes {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NoteEntity>(entity =>
        {
            entity.HasKey(n => n.id);

            entity.Property(n => n.title).IsRequired();

            entity.Property(n => n.body).IsRequired();
        });
    }
}