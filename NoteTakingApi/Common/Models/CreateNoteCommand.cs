using FluentValidation;
using NoteTakingApi.Common.Constants;
using NoteTakingApi.Common.Exceptions;

namespace NoteTakingApi.Common.Models;

public record CreateNoteCommand(string Title, string Content, List<string> Tags);


public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithErrorCode(ValidationErrors.Empty);
        RuleFor(x => x.Title)
            .NotNull().Length(1, DatabaseConstants.TITLE_MAX_LENGTH)
            .WithErrorCode(ValidationErrors.Invalid);
        
        RuleFor(x => x.Content)
            .NotEmpty().WithErrorCode(ValidationErrors.Empty);
        RuleFor(x => x.Content)
            .NotNull().Length(1, DatabaseConstants.CONTENT_MAX_LENGTH)
            .WithErrorCode(ValidationErrors.Invalid);
    }
}