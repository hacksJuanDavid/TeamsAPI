using FluentValidation;
using TeamsApi.Dtos;

namespace TeamsApi.Validations;

public class TeamValidator : AbstractValidator<TeamDto>
{
    public TeamValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(50) // Maximum length of 50 characters
            .Must(BeValidString) // Validate that the string is not null or empty and that it contains only letters or white spaces
            .WithMessage("Name must be a valid string with a maximum length of 50 characters.");

        RuleFor(m => m.Coach)
            .NotEmpty()
            .MaximumLength(50) // Maximum length of 50 characters 
            .Must(BeValidString) // Validate that the string is not null or empty and that it contains only letters or white spaces
            .WithMessage("Coach must be a valid string with a maximum length of 50 characters.");
    }

    private bool BeValidString(string value)
    {
        // Validate that the string is not null or empty and that it contains only letters or white spaces
        return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit);
    }
}