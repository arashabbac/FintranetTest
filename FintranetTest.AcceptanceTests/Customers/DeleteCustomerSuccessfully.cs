using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;
using System.Net.Http.Json;
using FluentAssertions;
using FintranetTest.Common;

namespace FintranetTest.AcceptanceTests.Customers;

[Story(
        AsA = "As an admin",
        IWant = "I want to delete an existed customer")]
public class DeleteCustomerSuccessfully : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private int _customerId;
    private HttpStatusCode _statusCode;

    public DeleteCustomerSuccessfully(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("We have a customer")]
    async Task We_Have_A_Customer()
    {
        var customerViewModel = new
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09351008895",
            Email = Faker.Internet.Email("arash"),
            BankAccountNumber = "1233652114521",
        };

        using var response = await _httpClient.PostAsJsonAsync("/api/customers", customerViewModel);
        _customerId = (await response.Content.ReadFromJsonAsync<APIResponseModel<int>>()).Data;
    }

    [When(StepTitle = "Admin tries to delete it")]
    async Task Deleting_The_Customer()
    {
        using var response = await _httpClient.DeleteAsync($"/api/customers/{_customerId}");

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then(StepTitle = "Customer deleted successfully")]
    void Customer_Deleted_Successfully()
    {
        _statusCode.Should().Be(HttpStatusCode.OK);
        _apiResponse.IsSuccess.Should().BeTrue();
    }

    [Then("Get customer and verify its deletion")]
    async Task Get_And_Verify_The_Deletion_Of_Customer()
    {
        using var response = await _httpClient.GetAsync($"/api/customers/{_customerId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();

        apiResponse.IsSuccess.Should().BeFalse();
        apiResponse.Messages.Should().Contain("Customer not found");
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.We_Have_A_Customer())
            .When(c => c.Deleting_The_Customer())
            .Then(c => c.Customer_Deleted_Successfully())
            .Then(c => c.Get_And_Verify_The_Deletion_Of_Customer())
            .BDDfy();
    }
}

