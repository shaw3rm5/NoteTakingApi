using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Exceptions;
using NoteTakingApi.Infrastructure.Database;

namespace NoteTakingApi.Features.Notes;

public class GetNoteById
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/notes/{id:int}", Handle)
                .RequireAuthorization()
                .WithTags("Notes")
                .Produces<GetByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<IResult> Handle(
            int id,
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = await dbContext.Notes
                .AsNoTracking()
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId && !n.IsDeleted, cancellationToken);

            if (note is null)
                throw new NoteNotFindException(ErrorCodes.NotFound, $"note with id {id} not found");
            
            var result = new GetByIdResponse(
                note.Id,
                note.Title,
                note.Content,
                note.NoteTags.Select(nt => nt.Tag.Name).ToList(),
                note.CreatedAt,
                note.UpdatedAt
            );

            return Results.Ok(result);
        }
    }
    private record GetByIdResponse(int Id, string Title, string Content, List<string> Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
}