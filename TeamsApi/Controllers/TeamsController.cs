using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeamsApi.Dtos;
using TeamsApi.Exceptions;
using TeamsApi.Models;
using TeamsApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeamsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;
        private readonly IValidator<TeamDto> _teamValidator;

        public TeamsController(ITeamService teamService, IMapper mapper, IValidator<TeamDto> teamValidator)
        {
            _teamService = teamService;
            _mapper = mapper;
            _teamValidator = teamValidator;
        }

        // GET: api/<TeamsController>
        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeams();
            return Ok(_mapper.Map<List<Team>, List<TeamDto>>(teams));
        }

        // GET api/<TeamsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _teamService.GetTeamById(id);

            // Check if team exists
            if (team == null)
            {
                throw new NotFoundException($"Team with id {id} does not exist");
            }

            return Ok(_mapper.Map<Team, TeamDto>(team));
        }

        // POST api/<TeamsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TeamDto team)
        {
            try
            {
                // Validate TeamDto
                var validationResult = await _teamValidator.ValidateAsync(team);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }

                return Ok(await _teamService.CreateTeam(_mapper.Map<TeamDto, Team>(team)));
            }
            catch (ValidationException ex)
            {
                // Handle FluentValidation ValidationException
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }

        // PUT api/<TeamsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TeamDto team)
        {
            try
            {
                // Validate TeamDto
                var validationResult = await _teamValidator.ValidateAsync(team);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }

                // Check if team exists
                var existingTeam = await _teamService.GetTeamById(id);
                if (existingTeam == null)
                {
                    throw new NotFoundException($"Team with id {id} does not exist");
                }

                return Ok(await _teamService.UpdateTeam(_mapper.Map<TeamDto, Team>(team)));
            }
            catch (ValidationException ex)
            {
                // Handle FluentValidation ValidationException
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }

        // DELETE api/<TeamsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamService.DeleteTeam(id);
            return Ok(new { Message = $"Team with id {id} has been deleted." });
        }

        // GET api/<TeamsController>/team/5/members
        [HttpGet("{id}/members")]
        public async Task<IActionResult> GetTeamMembersByTeamId(int id)
        {
            var teamMembers = await _teamService.GetTeamMembersByTeamId(id);

            // Check if team exists
            if (teamMembers == null)
            {
                throw new NotFoundException($"Team with id {id} does not exist");
            }

            return Ok(_mapper.Map<List<TeamMember>, List<TeamMemberDto>>(teamMembers));
        }
    }
}