using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Common.Constants;

namespace NoteTakingApi.Infrastructure.Database.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userBuilder)
    {
        userBuilder.ToTable("Users");
        
        userBuilder
            .HasKey(u => u.Id);
        
        userBuilder
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();userBuilder
            .Property(u => u.Email)
            .HasMaxLength(DatabaseConstants.EMAIL_MAX_LENGTH);
        
        userBuilder
            .Property(u => u.PasswordHash)
            .HasMaxLength(DatabaseConstants.HASH_MAX_LENGTH);
        
        userBuilder
            .Property(u => u.FullName)
            .HasMaxLength(DatabaseConstants.FULLNAME_MAX_LENGTH);
        
        userBuilder
            .HasMany(u => u.Notes)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId);
    }
}