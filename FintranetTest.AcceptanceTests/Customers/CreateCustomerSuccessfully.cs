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
        IWant = "I want to create a new customer",
        SoThat = "So that I can have my customers list")]
public class CreateCustomerSuccessfully : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private object _customerViewModel;
    private APIResponseModel _apiResponse;
    private HttpStatusCode _statusCode;

    public CreateCustomerSuccessfully(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("Customer data were provided correctly")]
    void Customer_Data_Were_Provided_Correctly()
    {
        _customerViewModel = new
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09351008895",
            Email = Faker.Internet.Email("arash"),
            BankAccountNumber = "1233652114521",
        };
    }

    [When(StepTitle = "Calling Post Api")]
    async Task Calling_Post_Api()
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/customers", _customerViewModel);

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then(StepTitle = "Customer created successfully")]
    void Customer_Created_Successfully()
    {
        _statusCode.Should().Be(HttpStatusCode.OK);
        _apiResponse.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.Customer_Data_Were_Provided_Correctly())
            .When(c => c.Calling_Post_Api())
            .Then(c => c.Customer_Created_Successfully())
            .BDDfy();
    }
}
