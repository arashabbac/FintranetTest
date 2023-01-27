using FintranetTest.Domain.Contracts;
using FintranetTest.Domain.ValueObjects;
using FintranetTest.Presentation.Server.Commands;
using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Server.CommandHandlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<int>>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<int>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        //*************************************************
        var foundedCustomer = await _customerRepository.FindAsync(request.Id, cancellationToken);

        if (foundedCustomer is null)
            return Result.Fail<int>("Customer not found");
        //*************************************************

        var firstnameResult = Name.Create(request.Firstname);
        var lastnameResult = Name.Create(request.Lastname);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var bankAccounNumbertResult = BankAccountNumber.Create(request.BankAccountNumber);
        var emailResult = Email.Create(request.Email);

        var result = Result.Merge(firstnameResult, lastnameResult, phoneNumberResult, bankAccounNumbertResult, emailResult);

        if (result.IsFailed)
            return Result.Fail<int>(result.Errors);

        //*************************************************
        var updateCustomerResult = foundedCustomer.Update(firstnameResult.Value,
            lastnameResult.Value,
            DateOnly.FromDateTime(request.DateOfBirth.Value),
            phoneNumberResult.Value,
            emailResult.Value,
            bankAccounNumbertResult.Value,
            _customerRepository);

        if (updateCustomerResult.IsFailed)
            return Result.Fail<int>(updateCustomerResult.Errors);
        //*************************************************

        await _customerRepository.SaveAsync(cancellationToken);

        return foundedCustomer.Id;
    }
}
