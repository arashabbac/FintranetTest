using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FintranetTest.Presentation.Server.Infrastructures;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    public BaseController(ISender sender)
    {
        Sender = sender;
    }

    protected ISender Sender { get; }

    protected IActionResult APIResult<T>(Result<T> result)
    {
        if (result.IsFailed)
            return BadRequest(result.ToAPIResponse());

        return Ok(result.ToAPIResponse());
    }

    protected IActionResult APIResult(Result result)
    {
        if (result.IsFailed)
            return BadRequest(result.ToAPIResponse());

        return Ok(result.ToAPIResponse());
    }
}
