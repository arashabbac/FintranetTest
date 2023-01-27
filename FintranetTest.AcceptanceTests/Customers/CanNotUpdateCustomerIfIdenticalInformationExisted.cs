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
public class CanNotUpdateCustomerIfIdenticalInformationExisted : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private APIResponseModel _apiResponse;
    private int _customerId;
    private HttpStatusCode _statusCode;

    private readonly CustomerFormViewModel _customer1 = new()
    {
        Firstname = Faker.Name.First(),
        Lastname = Faker.Name.Last(),
        DateOfBirth = Faker.Identification.DateOfBirth(),
        PhoneNumber = "09351008895",
        Email = Faker.Internet.Email(),
        BankAccountNumber = "1233652114521",
    };

    private readonly CustomerFormViewModel _customer2 = new()
    {
        Firstname = Faker.Name.First(),
        Lastname = Faker.Name.Last(),
        DateOfBirth = Faker.Identification.DateOfBirth(),
        PhoneNumber = "09351008895",
        Email = Faker.Internet.Email(),
        BankAccountNumber = "1233652114521",
    };

    public CanNotUpdateCustomerIfIdenticalInformationExisted(TestingWebAppFactory<Program> factory)
        => _httpClient = factory.CreateClient();


    [Given("We have 2 customers")]
    async Task We_Have_Two_Customer()
    {
        using var response1 = await _httpClient.PostAsJsonAsync("/api/customers", _customer1);
        using var response2 = await _httpClient.PostAsJsonAsync("/api/customers", _customer2);
        _customerId = (await response2.Content.ReadFromJsonAsync<APIResponseModel<int>>()).Data;
    }

    [When(StepTitle = "Changing customer2 information with same identical information of customer1")]
    async Task Changing_Customer2_Information_With_Customer1_Information()
    {
        _customer2.Firstname = _customer1.Firstname;
        _customer2.Lastname = _customer1.Lastname;
        _customer2.DateOfBirth = _customer1.DateOfBirth;

        using var response = await _httpClient.PutAsJsonAsync($"/api/customers/{_customerId}", _customer2);

        _apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel>();
        _statusCode = response.StatusCode;
    }

    [Then("An error should be displayed: `Customer with this information already exists`")]
    void An_Error_Should_Be_Displayed()
    {
        _apiResponse.IsSuccess.Should().BeFalse();
        _statusCode.Should().Be(HttpStatusCode.BadRequest);
        _apiResponse.Messages.Should().Contain("Customer with this information already exists");
    }

    [Fact]
    public void Execute()
    {
        this.Given(c => c.We_Have_Two_Customer())
            .When(c => c.Changing_Customer2_Information_With_Customer1_Information())
            .Then(c => c.An_Error_Should_Be_Displayed())
            .BDDfy();
    }
}
