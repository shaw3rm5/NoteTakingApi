using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Infrastructure.Database;

namespace NoteTakingApi.Features.Tags;

public class GetTags
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/tags", Handle)
                .RequireAuthorization()
                .WithTags("Tags");
        }
        
        private static async Task<IResult> Handle(
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var existingUser = await dbContext.Users
                .Where(u => u.Id == userId).
                FirstOrDefaultAsync(cancellationToken);
            if (existingUser is null)
                return Results.Unauthorized();

            var notes = await dbContext.Tags.ToArrayAsync(cancellationToken);
                
            return Results.Ok(new
            {
                notes
            });
        }
    }
}