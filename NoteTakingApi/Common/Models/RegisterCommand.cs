using FluentValidation;

namespace NoteTakingApi.Common.Models;

public record RegisterCommand(string Email, string Password, string FullName);


public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().Matches("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
        RuleFor(x => x.Password)
            .NotNull().NotEmpty().Matches("^[^@\\s]+$").Length(1, 300);
        RuleFor(x => x.FullName)
            .NotNull().NotEmpty().Matches("^[^@\\s]+$").Length(1, 30);
    }
}