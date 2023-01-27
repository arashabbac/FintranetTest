using FintranetTest.Common.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FintranetTest.Presentation.Client.Pages.Customers;

public partial class FormEditor
{

    [Parameter]
    public EventCallback OnValidSubmit { get; set; }

    [Parameter]
    public CustomerFormViewModel Model { get; set; }

    private void HandleValidSubmit()
    {
        OnValidSubmit.InvokeAsync(null);
    }
}
