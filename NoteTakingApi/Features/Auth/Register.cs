using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using NoteTakingApi.Infrastructure.Services;

namespace NoteTakingApi.Features.Auth;

public class Register
{
    public static class Endpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/auth/register", Handle)
                .WithName("HelloWorld") // важно для генерации имени эндпоинта
                .WithTags("Example");

        private static async Task<IResult> Handle(
            RegisterCommand command,
            IValidator<RegisterCommand> validator,
            ApplicationDbContext dbContext, 
            ILogger<Register> logger,
            CancellationToken cancellationToken)
        {
            logger.LogCritical("User with email {Email} registered", command.Email);
            await validator.ValidateAndThrowAsync(command, cancellationToken);
            
            var user = new User().Register(command.Email, command.Password, command.FullName, new PasswordHasher<User>());
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Created("user", user);
        }
    }
}