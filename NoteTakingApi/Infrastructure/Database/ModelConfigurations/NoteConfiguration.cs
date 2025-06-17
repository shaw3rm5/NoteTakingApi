using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApi.Common.Constants;
using NoteTakingApi.Common.Models;

namespace NoteTakingApi.Infrastructure.Database.ModelConfigurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> noteBuilder)
    {
        noteBuilder.ToTable("Notes");
        
        noteBuilder
            .HasKey(n => n.Id);
        noteBuilder
            .Property(n => n.Id)
            .ValueGeneratedOnAdd();

        noteBuilder
            .Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(DatabaseConstants.TITLE_MAX_LENGTH);

        noteBuilder
            .Property(n => n.Content)
            .IsRequired()
            .HasMaxLength(DatabaseConstants.CONTENT_MAX_LENGTH);
    }
}