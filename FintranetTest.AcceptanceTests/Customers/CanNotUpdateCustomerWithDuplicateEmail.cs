using System.Net.Http;
using System.Net;
using Xunit;
using TestStack.BDDfy;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FluentAssertions;
using FintranetTest.Common;
using FintranetTest.Common.ViewModels;

namespace FintranetTest.AcceptanceTests.Customers;

[Story(
        AsA = "As an admin",
        IWant = "I want to update an existed customer")]
public class CanNotUpdateCustomerWithDuplicateEmail : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private int _customerId;
    private HttpStatusCode _statusCode;

    private readonly string _arashEmail = Faker.Internet.Email("arash");
    private readonly string _johnEmail = Faker.Internet.Email("john");

    public CanNotUpdateCustomerWithDuplicateEmail(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("We have 2 customers")]
    async Task We_Have_A_Customer()
    {
        var customer1 = new CustomerFormViewModel
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09351008895",
            Email = _arashEmail,
            BankAccountNumber = "2255115566221"
        };

        var newCustomer = new CustomerFormViewModel
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09122221144",
            Email = _johnEmail,
            BankAccountNumber = "5465421231312"
        };

        using var response1 = await _httpClient.PostAsJsonAsync("/api/customers", customer1);
        using var response2 = await _httpClient.PostAsJsonAsync("/api/customers", newCustomer);
        _customerId = (await response2.Content.ReadFromJsonAsync<APIResponseModel<int>>()).Data;
    }

    [When(StepTitle = "Changing new customer email with same email of customer1")]
    async Task Changing_Customer_Email()
    {
        var customerViewModel = new CustomerFormViewModel
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09122221144",
            Email = _arashEmail,
            BankAccountNumber = "5465421231312"
        };

        using var response = await _httpClient.PutAsJsonAsync($"/api/customers/{_customerId}", customerViewModel);

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then("An error should be displayed: `Email has already been used`")]
    void An_Error_Should_Be_Displayed()
    {
        _apiResponse.IsSuccess.Should().BeFalse();
        _statusCode.Should().Be(HttpStatusCode.BadRequest);
        _apiResponse.Messages.Should().Contain("Email has already been used");
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.We_Have_A_Customer())
            .When(c => c.Changing_Customer_Email())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}