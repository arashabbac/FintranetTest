using System.Net.Http;
using System.Net;
using Xunit;
using TestStack.BDDfy;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System;
using FluentAssertions;
using FintranetTest.Common;

namespace FintranetTest.AcceptanceTests.Customers;

[Story(
        AsA = "As an admin",
        IWant = "I want to create a new customer",
        SoThat = "So that I can have my customers list")]
public class CanNotCreateCustomerWithSameIdenticalInformation : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private HttpStatusCode _statusCode;

    private readonly string _firstname = Faker.Name.First();
    private readonly string _lastname = Faker.Name.Last();
    private readonly DateTime _dateOfBtirh = Faker.Identification.DateOfBirth();

    public CanNotCreateCustomerWithSameIdenticalInformation(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();

    [Given("We add a customer with this information => Firstname: Arash, Lastname: Abbasi, DateOfBirth: 17/05/1993")]
    internal async Task Create_A_Customer()
    {
        using var request = await _httpClient.PostAsJsonAsync("/api/customers", new
        {
            Firstname = _firstname,
            Lastname = _lastname,
            DateOfBirth = _dateOfBtirh,
            PhoneNumber = "09351008895",
            Email = Faker.Internet.Email(),
            BankAccountNumber = "2255115566221"
        });
    }

    [When("Admin tries to create another customer with same identity information => Firstname: Arash, Lastname: Abbasi, DateOfBirth: 17/05/1993")]
    internal async Task Create_New_Customer_With_Same_Identity_Information()
    {
        using var request = await _httpClient.PostAsJsonAsync("/api/customers", new
        {
            Firstname = _firstname,
            Lastname = _lastname,
            DateOfBirth = _dateOfBtirh,
            PhoneNumber = "09352008895",
            Email = Faker.Internet.Email(),
            BankAccountNumber = "2255115566221"
        });

        _apiResponse = await request.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = request.StatusCode;
    }

    [Then("An error should be displayed: `Customer with this information already exists`")]
    internal void An_Error_Should_Be_Displayed()
    {
        _apiResponse.IsSuccess.Should().BeFalse();
        _apiResponse.Messages.Should().Contain("Customer with this information already exists");
        _statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.Create_A_Customer())
            .When(c => c.Create_New_Customer_With_Same_Identity_Information())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}
