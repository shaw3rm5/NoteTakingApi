using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using NoteTakingApi.Infrastructure.Services;

namespace NoteTakingApi.Features.Auth;

public class Authorization
{
    public static class Endpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", Handler)
                .WithTags("Auth")
                .AllowAnonymous();
        }

        private static async Task<IResult> Handler(
            LoginCommand command,
            ApplicationDbContext dbContext,
            IJwtService jwtService,
            IPasswordHasher<User> passwordHasher,
            CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

            if (user is null || user.VerifyPassword(command.Password, user.PasswordHash, passwordHasher))
            {
                return Results.Unauthorized();
                // todo custom exceptions
            }

            var token = jwtService.GenerateToken(user.Id, user.Email);
            return Results.Ok(new
            {
                token
            });
    }
    }
}