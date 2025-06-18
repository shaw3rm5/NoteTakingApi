using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Exceptions;
using NoteTakingApi.Infrastructure.Services;

namespace NoteTakingApi.Features.Auth;

public class Refresh
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/auth/refresh", Handle)
                .RequireAuthorization()
                .WithTags("Auth")
                .Produces<JwtRefreshResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<IResult> Handle(
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            IJwtService jwtService,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

            var existingUser = await dbContext.Users.FindAsync([userId], cancellationToken);
            if (existingUser is null)
                throw new NotAuthorizedException(ErrorCodes.Unauthorized, "User not found");
            var token = jwtService.GenerateToken(existingUser.Id, existingUser.Email);

            return Results.Ok(new
            {   
                token
            });
        }
    }
    record JwtRefreshResponse(string Token);
}