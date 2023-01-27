using FintranetTest.Common;
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
        IWant = "I want to delete an existed customer")]
public class CanNotDeleteCustomerIfIsNotExisted : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private HttpStatusCode _statusCode;
    private int _customerId;

    public CanNotDeleteCustomerIfIsNotExisted(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("I want to delete a customer and it doesn't exist")]
    void I_Want_To_Delete_A_Customer_And_It_Doesnot_Exist()
    {
        _customerId = Faker.RandomNumber.Next(100, 1000);
    }

    [When(StepTitle = "I call the DELETE API")]
    async Task Call_Delete_API()
    {
        using var response = await _httpClient.DeleteAsync($"/api/customers/{_customerId}");

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
        this.Given(c => c.I_Want_To_Delete_A_Customer_And_It_Doesnot_Exist())
            .When(c => c.Call_Delete_API())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}