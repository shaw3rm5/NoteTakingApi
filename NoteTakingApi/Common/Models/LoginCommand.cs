using FluentValidation;
using NoteTakingApi.Common.Exceptions;

namespace NoteTakingApi.Common.Models;

public record LoginCommand(string Email, string Password);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().Matches("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$")
            .WithErrorCode(ValidationErrors.Invalid);
        RuleFor(x => x.Password)
            .NotNull().NotEmpty().Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$").Length(8, 300)
            .WithErrorCode(ValidationErrors.Invalid);
    }
}