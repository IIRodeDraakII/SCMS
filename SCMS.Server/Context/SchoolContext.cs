using Microsoft.EntityFrameworkCore;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options)
          : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryItem> LibraryItems { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Teacher>().ToTable("Teachers");
        modelBuilder.Entity<Class>().ToTable("Classes");
        modelBuilder.Entity<Library>().ToTable("Libraries");
        modelBuilder.Entity<LibraryItem>().ToTable("LibraryItems");
        modelBuilder.Entity<Event>().ToTable("Events");


    }
}