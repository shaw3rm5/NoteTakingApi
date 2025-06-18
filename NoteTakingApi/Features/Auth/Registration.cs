using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Exceptions;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using NoteTakingApi.Infrastructure.Services;

namespace NoteTakingApi.Features.Auth;

public class Registration
{
    public static class Endpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapPost("/auth/register", Handle)
                .WithTags("Auth")
                .Produces(StatusCodes.Status201Created, typeof(RegistrationResponse))
                .Produces(StatusCodes.Status409Conflict);

        private static async Task<IResult> Handle(
            RegisterCommand command,
            IValidator<RegisterCommand> validator,
            ApplicationDbContext dbContext, 
            ILogger<Registration> logger,
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);
            
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

            if (existingUser is not null)
                throw new UserAlreadyExistsException(ErrorCodes.Conflict, $"User with email {command.Email} already exists.");
            
            var user = new User().Register(command.Email, command.Password, command.FullName, new PasswordHasher<User>());
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Results.Created("user", new RegistrationResponse(user.Id, user.Email, user.FullName));
        }
    }
    record RegistrationResponse(int Id, string Email, string FullName);
}