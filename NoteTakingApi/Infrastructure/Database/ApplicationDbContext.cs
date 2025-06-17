using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Infrastructure.Database.ModelConfigurations;

namespace NoteTakingApi.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);
    }
}