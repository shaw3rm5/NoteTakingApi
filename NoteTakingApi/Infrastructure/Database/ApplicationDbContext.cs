using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database.ModelConfigurations;
using NoteTakingApi.Infrastructure.Entities;

namespace NoteTakingApi.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteTag> NoteTags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteConfiguration).Assembly);
    }
}