using FintranetTest.Common.ViewModels;
using FintranetTest.Presentation.Client.Proxies;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Client.Pages.Customers;

public partial class Index
{
    [Inject]
    public CustomerProxy CustomerProxy { get; set; }

    public IReadOnlyList<CustomerViewModel> Customers { get; set; }
    private bool _tableLoading;

    protected override async Task OnInitializedAsync()
    {
        await FetchCustomersAsync();
    }

    private async Task FetchCustomersAsync()
    {
        _tableLoading = true;

        Customers = await CustomerProxy.GetAllAsync();

        _tableLoading = false;
    }

    async Task DeleteCustomerAsync(int id)
    {
        await CustomerProxy.DeleteAsync(id);
        await FetchCustomersAsync();
    }
}
