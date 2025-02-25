using ApiSults.Application.TicketModule.Queries.ExportTickets;
using ApiSults.Application.TicketModule.Queries.GetTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSults.Presentation.Api.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Route("v1/tickets")]
public class TicketController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTickets([FromQuery] GetTicketsRequest request)
    {
        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || !response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet("export")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportTickets([FromQuery] ExportTicketsRequest request)
    {
        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || response.Length == 0)
            return NoContent();

        return File(response, "text/csv", "tickets.csv");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTicketById([FromRoute] long id)
    {
        var request = new GetTicketsRequest(Id: id);

        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || !response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet("export/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportTicketsById([FromRoute] long id)
    {
        var request = new ExportTicketsRequest(Id: id);

        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || response.Length == 0)
            return NoContent();

        return File(response, "text/csv", "tickets.csv");
    }

    [HttpGet("department/{departmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTicketsByDepartment([FromRoute] long departmentId)
    {
        var request = new GetTicketsRequest(Department: departmentId);

        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || !response.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet("department/export/{departmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportTicketsByDepartment([FromRoute] long departmentId)
    {
        var request = new ExportTicketsRequest(Department: departmentId);

        if (request.Invalid())
            return BadRequest();

        var response = await _sender.Send(request);

        if (response == null || response.Length == 0)
            return NoContent();

        return File(response, "text/csv", "tickets.csv");
    }
}
