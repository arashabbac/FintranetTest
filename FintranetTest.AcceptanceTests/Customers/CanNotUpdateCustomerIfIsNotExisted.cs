using FintranetTest.Common;
using FintranetTest.Common.ViewModels;
using FluentAssertions;
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
public class CanNotUpdateCustomerIfIsNotExisted : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private HttpStatusCode _statusCode;
    private int _customerId;

    private CustomerFormViewModel _customerViewModel;

    public CanNotUpdateCustomerIfIsNotExisted(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("I want to update a customer and it doesn't exist")]
    void I_Want_To_Update_A_Customer_And_It_Doesnot_Exist()
    {
        _customerViewModel = new()
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09351008895",
            Email = Faker.Internet.Email(),
            BankAccountNumber = "1233652114521",
        };

        _customerId = Faker.RandomNumber.Next(50, 100);
    }

    [When(StepTitle = "I call the PUT API")]
    async Task Call_Put_API()
    {
        using var response = await _httpClient.PutAsJsonAsync($"/api/customers/{_customerId}", _customerViewModel);

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then("An error should be displayed: `Customer not found`")]
    void An_Error_Should_Be_Displayed()
    {
        _apiResponse.IsSuccess.Should().BeFalse();
        _statusCode.Should().Be(HttpStatusCode.BadRequest);
        _apiResponse.Messages.Should().Contain("Customer not found");
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.I_Want_To_Update_A_Customer_And_It_Doesnot_Exist())
            .When(c => c.Call_Put_API())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}
