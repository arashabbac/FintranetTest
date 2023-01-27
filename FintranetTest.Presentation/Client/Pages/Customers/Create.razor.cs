using FintranetTest.Common.ViewModels;
using FintranetTest.Presentation.Client.Proxies;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Client.Pages.Customers;

public partial class Create
{
    [Inject]
    public CustomerProxy CustomerProxy { get; set; }

    public CustomerFormViewModel Model { get; set; }

    protected override void OnInitialized()
    {
        Model = new();
    }

    private async Task Submit() => await CustomerProxy.CreateAsync(Model);
}
