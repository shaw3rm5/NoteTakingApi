using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using NoteTakingApi.Infrastructure.Entities;

namespace NoteTakingApi.Features.Notes;

public class UpdateNote
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapPut("/notes/{id:int}", Handle)
                .RequireAuthorization()
                .WithTags("Notes");
        }

        private static async Task<IResult> Handle(
            int id,
            UpdateNoteCommand command,
            IValidator<UpdateNoteCommand> validator,
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);
            
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var note = await dbContext.Notes
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId && !n.IsDeleted, cancellationToken);

            if (note is null)
                return Results.NotFound();

            note.Update(command.Title, command.Content);

            dbContext.NoteTags.RemoveRange(note.NoteTags);

            var existingTags = await dbContext.Tags
                .Where(t => command.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var newTagNames = command.Tags.Except(existingTags.Select(t => t.Name)).Distinct();
            var newTags = newTagNames.Select(name => new Tag { Name = name }).ToList();

            dbContext.Tags.AddRange(newTags);
            await dbContext.SaveChangesAsync(cancellationToken); 

            var allTags = existingTags.Concat(newTags).ToList();

            note.NoteTags = allTags.Select(tag => new NoteTag
            {
                NoteId = note.Id,
                TagId = tag.Id
            }).ToList();

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
    }
}