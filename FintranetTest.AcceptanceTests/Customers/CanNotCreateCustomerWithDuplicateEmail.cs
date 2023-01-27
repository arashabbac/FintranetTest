using System.Net.Http;
using System.Net;
using Xunit;
using TestStack.BDDfy;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FluentAssertions;
using FintranetTest.Common;

namespace FintranetTest.AcceptanceTests.Customers;

[Story(
        AsA = "As an admin",
        IWant = "I want to create a new customer",
        SoThat = "So that I can have my customers list")]
public class CanNotCreateCustomerWithDuplicateEmail : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private HttpStatusCode _statusCode;
    private readonly string _email = Faker.Internet.Email();
    public CanNotCreateCustomerWithDuplicateEmail(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();

    [Given("We add a customer with this information => Email: araash.abs@gmail.com")]
    internal async Task Add_A_Customer()
    {
        using var request = await _httpClient.PostAsJsonAsync("/api/customers", new
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09351008895",
            Email = _email,
            BankAccountNumber = "2255115566221"
        });
    }

    [When("Admin tries to create another customer with this email => Email: araash.abs@gmail.com")]
    internal async Task Add_New_Customer_With_Same_Email()
    {
        using var request = await _httpClient.PostAsJsonAsync("/api/customers", new
        {
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            DateOfBirth = Faker.Identification.DateOfBirth(),
            PhoneNumber = "09352008895",
            Email = _email,
            BankAccountNumber = "2255115444221"
        });

        _apiResponse = await request.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = request.StatusCode;
    }

    [Then("An error should be displayed: `Email has already been used`")]
    internal void An_Error_Should_Be_Displayed()
    {
        _apiResponse.IsSuccess.Should().BeFalse();
        _apiResponse.Messages.Should().Contain("Email has already been used");
        _statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.Add_A_Customer())
            .When(c => c.Add_New_Customer_With_Same_Email())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}
