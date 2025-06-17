using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
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
                .WithTags("Auth");
        }

        private static async Task<IResult> Handle(
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            IJwtService jwtService,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var existingUser = await dbContext.Users.FindAsync([userId], cancellationToken);
            if (existingUser is null)
                return Results.Unauthorized();
            //todo custom exception
            var token = jwtService.GenerateToken(existingUser.Id, existingUser.Email);

            return Results.Ok(new
            {   
                token
            });
        }
    }
}