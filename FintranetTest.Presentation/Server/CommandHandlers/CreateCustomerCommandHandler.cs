using FintranetTest.Domain.Aggregates;
using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.ValueObjects;
using FintranetTest.Presentation.Server.Commands;
using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Server.CommandHandlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<int>>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        var lastnameResult = Name.Create(request.Lastname);
        var firstnameResult = Name.Create(request.Firstname);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var bankAccounNumbertResult = BankAccountNumber.Create(request.BankAccountNumber);

        var result = Result.Merge(firstnameResult, lastnameResult, phoneNumberResult, bankAccounNumbertResult, emailResult);

        if (result.IsFailed)
            return Result.Fail<int>(result.Errors);

        //*************************************************
        var customerResult = Customer.Create(firstnameResult.Value,
            lastnameResult.Value,
            DateOnly.FromDateTime(request.DateOfBirth.Value),
            phoneNumberResult.Value,
            emailResult.Value,
            bankAccounNumbertResult.Value,
            _customerRepository);

        if (customerResult.IsFailed)
            return Result.Fail<int>(customerResult.Errors);
        //*************************************************

        await _customerRepository.AddAsync(customerResult.Value, cancellationToken);
        await _customerRepository.SaveAsync(cancellationToken);

        return customerResult.Value.Id;
    }
}
