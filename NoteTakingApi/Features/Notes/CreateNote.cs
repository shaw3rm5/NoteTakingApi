using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NoteTakingApi.Common.Models;
using NoteTakingApi.Infrastructure.Database;
using NoteTakingApi.Infrastructure.Entities;

namespace NoteTakingApi.Features.Notes;

public class CreateNote
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/notes", Handle)
                .RequireAuthorization()
                .WithTags("Notes")
                .Produces<CreatedNoteResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        private static async Task<IResult> Handle(
            CreateNoteCommand request,
            IValidator<CreateNoteCommand> validator,
            ClaimsPrincipal user,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var note = Note.Create(userId, request.Title, request.Content);

            var existingTags = await dbContext.Tags
                .AsNoTracking()
                .Where(t => request.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var existingTagNames = existingTags.Select(t => t.Name).ToHashSet();

            var newTags = request.Tags
                .Where(name => !existingTagNames.Contains(name))
                .Distinct()
                .Select(name => new Tag { Name = name })
                .ToList();

            foreach (var tag in newTags)
            {
                tag.Id = default;
            }
            
            dbContext.Tags.AddRange(newTags);
            await dbContext.SaveChangesAsync(cancellationToken); 
            
            var allTags = existingTags.Concat(newTags).ToList();

            foreach (var tag in allTags)
            {
                note.NoteTags.Add(new NoteTag
                {
                    Note = note,
                    Tag = tag
                });
            }

            dbContext.Notes.Add(note);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new CreatedNoteResponse(
                note.Id,
                note.Title,
                note.Content,
                allTags.Select(t => t.Name).ToList(),
                note.CreatedAt
            ));
        }
    }
}

record CreatedNoteResponse(int Id, string Title, string Content, List<string> Tags, DateTimeOffset CreatedAt);