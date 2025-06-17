using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApi.Common.Constants;
using NoteTakingApi.Infrastructure.Entities;

namespace NoteTakingApi.Infrastructure.Database.ModelConfigurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(DatabaseConstants.TAG_MAX_LENGTH); 

        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder
            .HasMany(t => t.NoteTags)
            .WithOne(nt => nt.Tag)
            .HasForeignKey(nt => nt.TagId);
    }
}