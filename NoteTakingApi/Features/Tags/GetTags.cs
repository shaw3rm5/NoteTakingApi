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
            app.MapDelete("/notes/{id:int}", Handle)
                .RequireAuthorization()
                .WithTags("Notes");
        }
        
        private static async Task<IResult> Handle(
            int id,
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = await dbContext.Tags.ToArrayAsync(cancellationToken);

            return Results.NoContent();
        }
    }
}