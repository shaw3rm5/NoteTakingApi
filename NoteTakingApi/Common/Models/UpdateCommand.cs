using FluentValidation;
using NoteTakingApi.Common.Constants;
using NoteTakingApi.Common.Exceptions;

namespace NoteTakingApi.Common.Models;

public record UpdateNoteCommand(string Title, string Content, List<string> Tags);


public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().NotNull().Length(1, DatabaseConstants.TITLE_MAX_LENGTH)
            .WithErrorCode(ValidationErrors.Invalid);
        RuleFor(x => x.Content)
            .NotEmpty().NotNull().Length(1, DatabaseConstants.CONTENT_MAX_LENGTH)
            .WithErrorCode(ValidationErrors.Invalid);
    }
}