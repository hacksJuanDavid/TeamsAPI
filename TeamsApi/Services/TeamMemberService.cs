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
        // Verifica si el equipo existe
        var team = await _appDbContext.Teams.FindAsync(teamMember.TeamId);
        if (team == null)
        {
            // Manejar el caso en el que el equipo no existe
            // Puedes lanzar una excepción o tomar otra acción adecuada.
            throw new InvalidOperationException("El equipo no existe.");
        }

        // Asigna el equipo al miembro
        team.Members.Add(teamMember);

        // Confirma la transacción y guarda los cambios en la base de datos
        await _appDbContext.SaveChangesAsync();

        return teamMember;
    }
    
    public async Task DeleteTeamMember(int id)
    {
        var original = await _appDbContext.Set<TeamMember>().FindAsync(id);

        if (original is null)
        {
            throw new Exception($"TeamMember with Id={id} Not Found");
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
            throw new Exception($"TeamMember with Id={teamMember.Id} Not Found");
        }

        _appDbContext.Entry(original).CurrentValues.SetValues(teamMember);
        await _appDbContext.SaveChangesAsync();
        return teamMember;
    }

    public async Task<List<TeamMember>> GetTeamMembersByTeamId(int teamId)
    {
        return await _appDbContext.Set<TeamMember>().Where(tm => tm.TeamId == teamId).ToListAsync();
    }
}