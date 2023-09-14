using Microsoft.EntityFrameworkCore;
using TeamsApi.Context;
using TeamsApi.Exceptions;
using TeamsApi.Models;

namespace TeamsApi.Services;

public class TeamMemberService : ITeamMemberService
{
    private readonly AppDbContext _appDbContext;

    public TeamMemberService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<TeamMember>> GetAllTeamMembers()
    {
        return await _appDbContext.Set<TeamMember>().ToListAsync();
    }

    public async Task<TeamMember?> GetTeamMemberById(int id)
    {
        return await _appDbContext.Set<TeamMember>().FindAsync(id);
    }

    public async Task<TeamMember> CreateTeamMember(TeamMember teamMember)
    {
        var result = await _appDbContext.Set<TeamMember>().AddAsync(teamMember);
        await _appDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<TeamMember> UpdateTeamMember(TeamMember teamMember)
    {
        var original = await _appDbContext.Set<TeamMember>().FindAsync(teamMember.Id);

        // Check if team member exists
        if (original is null)
        {
            throw new NotFoundException($"TeamMember with Id={teamMember.Id} Not Found");
        }

        // Check if team member has a team
        _appDbContext.Entry(original).CurrentValues.SetValues(teamMember);

        // Save changes
        await _appDbContext.SaveChangesAsync();

        return teamMember;
    }

    public async Task DeleteTeamMember(int id)
    {
        var original = await _appDbContext.Set<TeamMember>().FindAsync(id);

        // Check if team member exists
        if (original is null)
        {
            throw new NotFoundException($"TeamMember with Id={id} Not Found");
        }

        // Delete team member
        _appDbContext.Set<TeamMember>().Remove(original);

        // Save changes
        await _appDbContext.SaveChangesAsync();
    }


    // Get /members/{id}/teams
    public async Task<List<Team>> GetTeamsByMemberId(int id)
    {
        // Get team members
        var teamMembers = await _appDbContext.Set<TeamMember>().Where(tm => tm.Id == id).ToListAsync();
        // Create list of teams
        var teams = new List<Team>();

        // Get teams
        foreach (var teamMember in teamMembers)
        {
            var team = await _appDbContext.Set<Team>().FindAsync(teamMember.TeamId); // Get team
            if (team != null) teams.Add(team); // Add team to list
        }

        // Check if team member exists
        if (teams.Count == 0)
        {
            throw new NotFoundException($"Team member with id {id} does not exist");
        }

        return teams;
    }
}