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
            .NotNull().NotEmpty().Length(8, 300);
        RuleFor(x => x.FullName)
            .NotNull().NotEmpty().Length(5, 30);
    }
}