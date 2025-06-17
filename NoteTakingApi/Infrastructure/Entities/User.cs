using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace NoteTakingApi.Common.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public ICollection<Note> Notes {get; set; }

    public User() { }

    public User Register(string email, string password, string fullName, IPasswordHasher<User> passwordHasher)
    {
        var hashedPassword = passwordHasher.HashPassword(this, password);
        return new User
        {
            Email = email,
            PasswordHash = hashedPassword,
            FullName = fullName,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public bool VerifyPassword(string password, string hashedPassword, IPasswordHasher<User> passwordHasher) 
    => passwordHasher.VerifyHashedPassword(this,  hashedPassword, password) == PasswordVerificationResult.Success;
    
}