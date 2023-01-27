using FluentResults;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace FintranetTest.Presentation.Server.Commands;

public record CreateCustomerCommand(string Firstname,
    string Lastname,
    DateTime? DateOfBirth,
    string PhoneNumber,
    string Email,
    string BankAccountNumber) : IRequest<Result<int>>;

public record UpdateCustomerCommand(string Firstname,
    string Lastname,
    DateTime? DateOfBirth,
    string PhoneNumber,
    string Email,
    string BankAccountNumber) : IRequest<Result<int>>
{
    [JsonIgnore]
    public int Id { get; set; }
}

public record DeleteCustomerCommand(int Id) : IRequest<Result>;
