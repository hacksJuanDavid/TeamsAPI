using Microsoft.EntityFrameworkCore;
using TeamsApi.Context;
using TeamsApi.Exceptions;
using TeamsApi.Models;

namespace TeamsApi.Services
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _appDbContext;

        public TeamService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await _appDbContext.Set<Team>().ToListAsync();
        }

        public async Task<Team?> GetTeamById(int id)
        {
            return await _appDbContext.Set<Team>().FindAsync(id);
        }

        public async Task<Team> CreateTeam(Team team)
        {
            _appDbContext.Set<Team>().Add(team);
            await _appDbContext.SaveChangesAsync();
            return team;
        }

        public async Task<Team?> UpdateTeam(Team? team)
        {
            var id = team?.Id;
            var original = await _appDbContext.Set<Team>().FindAsync(id);

            // Check if team exists
            if (original is null)
            {
                throw new NotFoundException($"Team with Id={id} Not Found");
            }

            // Check if team name is unique
            if (team is null)
            {
                throw new NotFoundException($"Team with Id={id} Not Found");
            }

            // Check if team name is unique
            _appDbContext.Entry(original).CurrentValues.SetValues(team);
            // Save changes
            await _appDbContext.SaveChangesAsync();

            return team;
        }

        public async Task DeleteTeam(int id)
        {
            var original = await _appDbContext.Set<Team>().FindAsync(id);

            // Check if team exists
            if (original is null)
            {
                throw new NotFoundException($"Team with Id={id} Not Found");
            }

            // Delete team members first
            var teamMembers = await _appDbContext.Set<TeamMember>()
                .Where(tm => tm.TeamId == id)
                .ToListAsync();

            _appDbContext.Set<TeamMember>().RemoveRange(teamMembers);

            // Delete team
            _appDbContext.Set<Team>().Remove(original);

            // Save changes
            await _appDbContext.SaveChangesAsync();
        }

        // Get /teams/{teamId}/members
        public async Task<List<TeamMember>> GetTeamMembersByTeamId(int id)
        {
            // Get team
            var team = await _appDbContext.Set<Team>().FindAsync(id);

            // Check if team exists
            if (team is null)
            {
                throw new NotFoundException($"Team with id {id} does not exist");
            }

            // Get team members
            var teamMembers = await _appDbContext.Set<TeamMember>()
                .Where(tm => tm.TeamId == id)
                .ToListAsync();

            // Check if team has members
            if (teamMembers.Count == 0)
            {
                throw new NotFoundException($"Team with id {id} exists, but it has no members assigned");
            }

            return teamMembers;
        }
    }
}