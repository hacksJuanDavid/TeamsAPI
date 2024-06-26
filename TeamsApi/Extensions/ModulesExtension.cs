using AutoMapper;
using FluentValidation;
using TeamsApi.Dtos;
using TeamsApi.Mapping;
using TeamsApi.Services;
using TeamsApi.Validations;

namespace TeamsApi.Extensions;

public static class ModulesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ITeamMemberService, TeamMemberService>();

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }

    public static IServiceCollection AddFluentValidationAutoValidation(this IServiceCollection services)
    {
        // Add FluentValidation
        services.AddTransient<IValidator<TeamDto>, TeamValidator>();
        services.AddTransient<IValidator<TeamMemberDto>, TeamMemberValidator>();

        return services;
    }
}