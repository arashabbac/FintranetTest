using FintranetTest.Persistence.QueryRepositories;
using FintranetTest.Presentation.Server.Commands;
using FintranetTest.Presentation.Server.Infrastructures;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Server.Controllers;

public class CustomersController : BaseController
{
    private readonly ICustomerQueryRepository _customerQueryRepository;
    public CustomersController(ISender sender, ICustomerQueryRepository customerQueryRepository) : base(sender)
    {
        _customerQueryRepository = customerQueryRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCustomerCommand command)
    {
        var result = await Sender.Send(command);

        return APIResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateCustomerCommand command)
    {
        command.Id = id;

        var result = await Sender.Send(command);

        return APIResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Sender.Send(new DeleteCustomerCommand(id));

        return APIResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerQueryRepository.GetByIdAsync(id);

        if (result is null)
            return APIResult(Result.Fail("Customer not found"));

        return APIResult(Result.Ok(result));
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerQueryRepository.GetAllAsync();

        return APIResult(Result.Ok(result));
    }
}
