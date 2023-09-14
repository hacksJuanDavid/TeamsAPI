using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TeamsApi.Dtos;
using TeamsApi.Exceptions;
using TeamsApi.Models;
using TeamsApi.Services;

namespace TeamsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsMemberController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly IMapper _mapper;
        private readonly IValidator<TeamMemberDto> _teamMemberValidator;

        public TeamsMemberController(ITeamMemberService teamMemberService, IMapper mapper,
            IValidator<TeamMemberDto> teamMemberValidator)
        {
            _teamMemberService = teamMemberService;
            _mapper = mapper;
            _teamMemberValidator = teamMemberValidator;
        }

        // GET: api/<TeamsMemberController>
        [HttpGet]
        public async Task<IActionResult> GetAllTeamMembers()
        {
            var teamMembers = await _teamMemberService.GetAllTeamMembers();
            return Ok(_mapper.Map<List<TeamMember>, List<TeamMemberDto>>(teamMembers));
        }

        // GET api/<TeamsMemberController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamMemberById(int id)
        {
            var teamMember = await _teamMemberService.GetTeamMemberById(id);

            // Check if team member exists
            if (teamMember == null)
            {
                throw new NotFoundException($"Team with id {id} does not exist");
            }

            return Ok(_mapper.Map<TeamMember, TeamMemberDto>(teamMember));
        }

        // POST api/<TeamsMemberController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TeamMemberDto teamMember)
        {
            try
            {
                // Validate TeamMemberDto
                var validationResult = await _teamMemberValidator.ValidateAsync(teamMember);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { Erros = errors });
                }

                return Ok(await _teamMemberService.CreateTeamMember(
                    _mapper.Map<TeamMemberDto, TeamMember>(teamMember)));
            }
            catch (Exception ex)
            {
                // Handle FluentValidation ValidationException
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }

        // PUT api/<TeamsMemberController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TeamMemberDto teamMember)
        {
            try
            {
                // Validate TeamMemberDto
                var validationResult = await _teamMemberValidator.ValidateAsync(teamMember);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { Errors = errors });
                }
                
                // Check if team member exists
                var existingTeamMember = await _teamMemberService.GetTeamMemberById(id);
                if (existingTeamMember == null)
                {
                    throw new NotFoundException($"Team member with id {id} does not exist");
                }
                
                return Ok(await _teamMemberService.UpdateTeamMember(
                    _mapper.Map<TeamMemberDto, TeamMember>(teamMember)));
            }
            catch (Exception ex)
            {
                // Handle FluentValidation ValidationException
                return BadRequest(new { Errors = new List<string> { ex.Message } });
            }
        }

        // DELETE api/<TeamsMemberController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamMemberService.DeleteTeamMember(id);
            return Ok( new { Message = $"Team member with id {id} has been deleted." });
        }

        // Get /members/{id}/teams
        [HttpGet("{id}/teams")]
        public async Task<IActionResult> GetTeamsByMemberId(int id)
        {
            var teams = await _teamMemberService.GetTeamsByMemberId(id);
            return Ok(_mapper.Map<List<Team>, List<TeamDto>>(teams));
        }
    }
}