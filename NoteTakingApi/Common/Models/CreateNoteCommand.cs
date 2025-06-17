using FluentValidation;
using NoteTakingApi.Common.Constants;

namespace NoteTakingApi.Common.Models;

public record CreateNoteCommand(string Title, string Content, List<string> Tags);


public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().NotNull().Length(1, DatabaseConstants.TITLE_MAX_LENGTH);
        RuleFor(x => x.Content)
            .NotEmpty().NotNull().Length(1, DatabaseConstants.CONTENT_MAX_LENGTH);
    }
}