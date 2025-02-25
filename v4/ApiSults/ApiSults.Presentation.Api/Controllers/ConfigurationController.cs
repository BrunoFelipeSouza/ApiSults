using ApiSults.Application.ConfigurationModule.Commands;
using ApiSults.Application.ConfigurationModule.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiSults.Presentation.Api.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Route("v1/config")]
public class ConfigurationController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfiguration()
    {
        var response = await _sender.Send(new GetConfigurationRequest());

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateConfiguration([FromBody] UpdateConfigurationRequest request)
    {
        if (request.Invalid())
            return BadRequest();

        await _sender.Send(request);

        return NoContent();
    }
}
