using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Services;
using OrderService.Api.DTOs;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeadLettersController : ControllerBase
{
    private readonly IDeadLetterService _deadLetterService;

    public DeadLettersController(IDeadLetterService deadLetterService)
    {
        _deadLetterService = deadLetterService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeadLetterDto>>> GetAll()
    {
        IEnumerable<DeadLetterDto> deadLetters = await _deadLetterService.GetAllAsync();
        return Ok(deadLetters);
    }
}
