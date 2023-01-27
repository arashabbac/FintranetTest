using FintranetTest.Common.ViewModels;
using FintranetTest.Presentation.Client.Proxies;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Client.Pages.Customers;

public partial class Edit
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public CustomerProxy CustomerProxy { get; set; }

    public CustomerFormViewModel Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model = new();

        var data =
            await CustomerProxy.GetByIdAsync(Id);

        Model =
            new()
            {
                Id = Id,
                Email = data.Email,
                Lastname = data.Lastname,
                Firstname = data.Firstname,
                PhoneNumber = data.PhoneNumber,
                BankAccountNumber = data.BankAccountNumber,
                DateOfBirth = data.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            };
    }

    private async Task Submit() => await CustomerProxy.UpdateAsync(Model);
}