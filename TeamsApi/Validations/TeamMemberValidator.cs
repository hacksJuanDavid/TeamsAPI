using FluentValidation;
using TeamsApi.Dtos;

namespace TeamsApi.Validations;

public class TeamMemberValidator: AbstractValidator<TeamMemberDto>
{
    public TeamMemberValidator()
    {
        RuleFor(m  => m.FirstName).NotEmpty();
        RuleFor(m  => m.LastName).NotEmpty();
        RuleFor(m  => m.BirthDate).NotEmpty();
        RuleFor(m  => m.Phone).NotEmpty();
    }
}