using TeamsApi.Models;

namespace TeamsApi.Services;

public interface ITeamService
{
    // GetAllTeams
    Task<List<Team>> GetAllTeams();

    // GetTeamById
    Task<Team?> GetTeamById(int id);

    // CreateTeam
    Task<Team> CreateTeam(Team team);

    // UpdateTeam
    Task<Team?> UpdateTeam(Team? team);

    // DeleteTeam
    Task DeleteTeam(int id);

    // GetTeamMembersByTeamId
    Task<List<TeamMember>> GetTeamMembersByTeamId(int id);
}