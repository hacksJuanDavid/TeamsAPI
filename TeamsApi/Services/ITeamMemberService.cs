using TeamsApi.Models;

namespace TeamsApi.Services;

public interface ITeamMemberService
{
    // GetAllTeamMembers
    Task<List<TeamMember>> GetAllTeamMembers();

    // GetTeamMemberById
    Task<TeamMember?> GetTeamMemberById(int id);

    // CreateTeamMember
    Task<TeamMember> CreateTeamMember(TeamMember teamMember);

    // UpdateTeamMember
    Task<TeamMember> UpdateTeamMember(TeamMember teamMember);

    // DeleteTeamMember
    Task DeleteTeamMember(int id);

    // GetTeamsByMemberId
    Task<List<Team>> GetTeamsByMemberId(int id);
}