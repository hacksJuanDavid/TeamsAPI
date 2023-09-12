using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamsApi.Dtos;
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

        public TeamsMemberController(ITeamMemberService teamMemberService, IMapper mapper)
        {
            _teamMemberService = teamMemberService;
            _mapper = mapper;
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

            if (teamMember == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TeamMember, TeamMemberDto>(teamMember));
        }

        // POST api/<TeamsMemberController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TeamMemberDto teamMember)
        {
            return Ok(await _teamMemberService.CreateTeamMember(_mapper.Map<TeamMemberDto, TeamMember>(teamMember)));
        }

        // PUT api/<TeamsMemberController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TeamMemberDto teamMember)
        {
            return Ok(await _teamMemberService.UpdateTeamMember(_mapper.Map<TeamMemberDto, TeamMember>(teamMember)));
        }

        // DELETE api/<TeamsMemberController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamMemberService.DeleteTeamMember(id);
            return Ok();
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