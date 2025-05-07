using APBDKolokwium1B.Models.DTOs;
using APBDKolokwium1B.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDKolokwium1B.Controllers;
[Route("api/[controller]")]
[ApiController]

public class VisitsController : ControllerBase
{
    private readonly IDbService _dbService;
    public VisitsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("visits/{id}")]
    public async Task<IActionResult> GetVisits(int id)
    {
        try
        {
            var res = await _dbService.GetVisitDataByIdAsync(id);
            return Ok(res);
        }
        catch (NotFoundException e)
        {
            return NotFound(e);
        }
    }

    [HttpPost("visits")]
    public async Task<IActionResult> AddNewVisit(string licence, CreateVisitDto createVisitDto)
    {
        return CreatedAtAction(nameof(GetVisits),new {licence}, createVisitDto);
    }
}