using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Exceptions;
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
                throw new NoteNotFindException(ErrorCodes.NotFound, $"note with id {id} not found");

            note.Update(command.Title, command.Content);

            var existingTags = await dbContext.Tags
                .Where(t => command.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var existingTagNames = existingTags.Select(t => t.Name).ToList();

            var newTags = command.Tags
                .Where(name => !existingTagNames.Contains(name))
                .Distinct()
                .Select(name => new Tag { Name = name })
                .ToList();

            if (newTags.Any())
                dbContext.Tags.AddRange(newTags);
            var allTags = existingTags.Concat(newTags).ToList();

            note.NoteTags.Clear();

            foreach (var tag in allTags)
            {
                note.NoteTags.Add(new NoteTag
                {
                    Note = note,
                    Tag = tag
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
    }
}