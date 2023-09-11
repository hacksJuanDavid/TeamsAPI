using TeamsApi.Models;

namespace TeamsApi.Services;

public interface ITeamMemberService
{
    // CreateTeamMember
    Task<TeamMember> CreateTeamMember(TeamMember teamMember);
    
    // DeleteTeamMember
    Task DeleteTeamMember(int id);
    
    // GetAllTeamMembers
    Task<List<TeamMember>> GetAllTeamMembers();
    
    // GetTeamMemberById
    Task<TeamMember?> GetTeamMemberById(int id);
    
    // UpdateTeamMember
    Task<TeamMember> UpdateTeamMember(TeamMember? teamMember);
    
    // GetTeamMembersByTeamId
    Task<List<TeamMember>> GetTeamMembersByTeamId(int teamId);
    
}