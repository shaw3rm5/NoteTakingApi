using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Infrastructure.Database;

namespace NoteTakingApi.Features.Notes;

public class GetNotes
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/notes", Handle)
                .RequireAuthorization()
                .WithTags("Notes")
                .Produces<ResponseNote[]>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<IResult> Handle(
            string? tag,
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = dbContext.Notes
                .AsNoTracking()
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tag))
                query = query.Where(n =>
                    n.NoteTags.Any(nt => nt.Tag.Name.ToLower() == tag.ToLower()));
            

            var notes = await query
                .OrderByDescending(n => n.CreatedAt)
                .ToArrayAsync(cancellationToken);

            var result = notes.Select(n => new ResponseNote(
                n.Id,
                n.Title,
                n.Content,
                n.NoteTags.Select(nt => nt.Tag.Name).ToList(),
                n.CreatedAt,
                n.UpdatedAt
            )).ToArray();

            return Results.Ok(result);
        }
    }
}

record ResponseNote(int Id, string Title, string Content, List<string> Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);