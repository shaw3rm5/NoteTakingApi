using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Infrastructure.Database;

namespace NoteTakingApi.Features.Notes;

public class DeleteNote
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
            ApplicationDbContext db,
            CancellationToken ct)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = await db.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId && !n.IsDeleted, ct);

            if (note is null)
                return Results.NotFound();

            note.Delete();

            await db.SaveChangesAsync(ct);

            return Results.NoContent();
        }
    }
}