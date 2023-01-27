using FintranetTest.Common;
using FintranetTest.Common.ViewModels;
using FintranetTest.Persistence.DTOs;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace FintranetTest.AcceptanceTests.Customers;

[Story(
        AsA = "As an admin",
        IWant = "I want to update an existed customer")]
public class UpdateCustomerSuccessfully : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private int _customerId;
    private HttpStatusCode _statusCode;

    private readonly CustomerFormViewModel _customer = new()
    {
        Firstname = Faker.Name.First(),
        Lastname = Faker.Name.Last(),
        DateOfBirth = Faker.Identification.DateOfBirth(),
        PhoneNumber = "09351008895",
        Email = Faker.Internet.Email(),
        BankAccountNumber = "1233652114521",
    };

    private readonly CustomerFormViewModel _updatedCustomer = new()
    {
        Firstname = Faker.Name.First(),
        Lastname = Faker.Name.Last(),
        DateOfBirth = Faker.Identification.DateOfBirth(),
        PhoneNumber = "09351008895",
        Email = Faker.Internet.Email(),
        BankAccountNumber = "1233652114521",
    };

    public UpdateCustomerSuccessfully(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("We have a customer")]
    async Task We_Have_A_Customer()
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/customers", _customer);
        _customerId = (await response.Content.ReadFromJsonAsync<APIResponseModel<int>>()).Data;
    }

    [When(StepTitle = "Changing the customer information")]
    async Task Changing_Customer_Information()
    {
        using var response = await _httpClient.PutAsJsonAsync($"/api/customers/{_customerId}", _updatedCustomer);

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then(StepTitle = "Customer updated successfully")]
    void Customer_Updated_Successfully()
    {
        _statusCode.Should().Be(HttpStatusCode.OK);
        _apiResponse.IsSuccess.Should().BeTrue();
    }

    [Then("Get updated customer and verify its changes")]
    async Task Get_And_Verify_Customer_Changes()
    {
        var response = await _httpClient.GetFromJsonAsync<APIResponseModel<CustomerDto>>($"/api/customers/{_customerId}");

        response.Data.Should().NotBeNull();
        response.Data.Firstname.Should().Be(_updatedCustomer.Firstname.ToLower());
        response.Data.Lastname.Should().Be(_updatedCustomer.Lastname.ToLower());
        response.Data.Email.Should().Be(_updatedCustomer.Email.ToLower());
        response.Data.DateOfBirth.Should().Be(DateOnly.FromDateTime(_updatedCustomer.DateOfBirth.Value));
        response.Data.BankAccountNumber.Should().Be(_updatedCustomer.BankAccountNumber);
        response.Data.PhoneNumber.Should().Be(_updatedCustomer.PhoneNumber);
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.We_Have_A_Customer())
            .When(c => c.Changing_Customer_Information())
            .Then(c => c.Customer_Updated_Successfully())
            .Then(c => c.Get_And_Verify_Customer_Changes())
            .BDDfy();
    }
}
