using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _service;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            var sponsors = await _service.GetAllAsync();
            var response = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            var sponsor = await _service.GetByIdAsync(id);

            if (sponsor == null)
                return NotFound();

            var response = _mapper.Map<SponsorResponseDTO>(sponsor);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> Create([FromBody] SponsorRequestDTO request)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(request);
                var created = await _service.CreateAsync(sponsor);
                var response = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SponsorRequestDTO request)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(request);
                await _service.UpdateAsync(id, sponsor);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/tournaments")]
        public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetTournamentsBySponsor(int id)
        {
            try
            {
                var tournamentSponsors = await _service.GetTournamentsBySponsorAsync(id);
                var response = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(tournamentSponsors);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/tournaments")]
        public async Task<ActionResult<TournamentSponsorResponseDTO>> LinkTournament(int id, [FromBody] TournamentSponsorRequestDTO request)
        {
            try
            {
                var tournamentSponsor = await _service.LinkTournamentAsync(id, request.TournamentId, request.ContractAmount);
                var response = _mapper.Map<TournamentSponsorResponseDTO>(tournamentSponsor);

                return CreatedAtAction(nameof(GetTournamentsBySponsor), new { id }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/tournaments/{tid}")]
        public async Task<IActionResult> UnlinkTournament(int id, int tid)
        {
            try
            {
                await _service.UnlinkTournamentAsync(id, tid);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
