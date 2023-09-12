using Microsoft.EntityFrameworkCore;
using TeamsApi.Context;
using TeamsApi.Models;

namespace TeamsApi.Services;

public class TeamMemberService : ITeamMemberService
{
    private readonly AppDbContext _appDbContext;

    public TeamMemberService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TeamMember> CreateTeamMember(TeamMember teamMember)
    {
        var result = await _appDbContext.Set<TeamMember>().AddAsync(teamMember);
        await _appDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task DeleteTeamMember(int id)
    {
        var original = await _appDbContext.Set<TeamMember>().FindAsync(id);

        if (original is null)
        {
            throw new ArgumentNullException($"TeamMember with Id={id} Not Found");
        }

        _appDbContext.Set<TeamMember>().Remove(original);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<TeamMember>> GetAllTeamMembers()
    {
        return await _appDbContext.Set<TeamMember>().ToListAsync();
    }

    public async Task<TeamMember?> GetTeamMemberById(int id)
    {
        return await _appDbContext.Set<TeamMember>().FindAsync(id);
    }

    public async Task<TeamMember> UpdateTeamMember(TeamMember teamMember)
    {
        var original = await _appDbContext.Set<TeamMember>().FindAsync(teamMember.Id);

        if (original is null)
        {
            throw new ArgumentNullException($"TeamMember with Id={teamMember.Id} Not Found");
        }

        _appDbContext.Entry(original).CurrentValues.SetValues(teamMember);
        await _appDbContext.SaveChangesAsync();
        return teamMember;
    }

    // Get /members/{id}/teams
    public async Task<List<Team>> GetTeamsByMemberId(int id)
    {
        var teamMembers = await _appDbContext.Set<TeamMember>().Where(tm => tm.Id == id).ToListAsync();
        var teams = new List<Team>();

        foreach (var teamMember in teamMembers)
        {
            var team = await _appDbContext.Set<Team>().FindAsync(teamMember.TeamId);
            if (team != null) teams.Add(team);
        }

        return teams;
    }
}