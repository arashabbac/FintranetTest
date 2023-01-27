using FintranetTest.Domain.Contracts;
using FintranetTest.Presentation.Server.Commands;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Server.CommandHandlers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        //*************************************************
        var foundedCustomer = await _customerRepository.FindAsync(request.Id, cancellationToken);

        if (foundedCustomer is null)
            return Result.Fail("Customer not found");
        //*************************************************

        _customerRepository.Delete(foundedCustomer);
        await _customerRepository.SaveAsync(cancellationToken);

        return new Result().WithSuccess("Customer has been deleted successfully");
    }
}
