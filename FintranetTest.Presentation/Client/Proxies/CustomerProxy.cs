using AntDesign;
using FintranetTest.Common;
using FintranetTest.Common.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FintranetTest.Presentation.Client.Proxies;

public class CustomerProxy
{
    private readonly HttpClient _httpClient;
    private readonly NotificationService _notificationService;
    private readonly NavigationManager _navigationManager;

    public CustomerProxy(HttpClient httpClient, NotificationService notificationService, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _notificationService = notificationService;
        _navigationManager = navigationManager;
    }


    public bool Loading { get; set; }

    public async Task CreateAsync(CustomerFormViewModel viewModel)
    {
        try
        {
            Loading = true;

            var response = await _httpClient.PostAsJsonAsync("customers", viewModel);

            if (response.IsSuccessStatusCode == false)
                await ShowErrorMessage(response);
            else
                _navigationManager.NavigateTo("/customers");
        }
        catch (System.Exception ex)
        {
            await ShowErrorMessage(ex.Message);
        }
        finally
        {
            Loading = false;
        }
    }

    public async Task UpdateAsync(CustomerFormViewModel viewModel)
    {
        try
        {
            Loading = true;

            var response = await _httpClient.PutAsJsonAsync($"customers/{viewModel.Id}", viewModel);

            if (response.IsSuccessStatusCode == false)
                await ShowErrorMessage(response);
            else
                _navigationManager.NavigateTo("/customers");
        }
        catch (System.Exception ex)
        {
            await ShowErrorMessage(ex.Message);
        }
        finally
        {
            Loading = false;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            Loading = true;

            var response = await _httpClient.DeleteAsync($"customers/{id}");

            if (response.IsSuccessStatusCode == false)
                await ShowErrorMessage(response);
            else
                await ShowSuccessMessage(response);
        }
        catch (System.Exception ex)
        {
            await ShowErrorMessage(ex.Message);
        }
        finally
        {
            Loading = false;
        }
    }

    public async Task<List<CustomerViewModel>> GetAllAsync()
    {
        try
        {
            Loading = true;
            var response = await _httpClient.GetAsync($"customers/get-all");

            if (response.IsSuccessStatusCode == false)
            {
                await ShowErrorMessage(response);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<APIResponseModel<List<CustomerViewModel>>>();

            return result.Data;
        }
        catch (System.Exception ex)
        {
            await ShowErrorMessage(ex.Message);
            return null;
        }
        finally
        {
            Loading = false;
        }
    }

    public async Task<CustomerViewModel> GetByIdAsync(int id)
    {
        try
        {
            Loading = true;

            var response = await _httpClient.GetAsync($"customers/{id}");

            if (response.IsSuccessStatusCode == false)
            {
                await ShowErrorMessage(response);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<APIResponseModel<CustomerViewModel>>();

            return result.Data;
        }
        catch (System.Exception ex)
        {
            await ShowErrorMessage(ex.Message);
            return null;
        }
        finally
        {
            Loading = false;
        }
    }

    private async Task ShowErrorMessage(HttpResponseMessage response)
    {
        foreach (var message in (await response.Content.ReadFromJsonAsync<APIResponseModel>()).Messages)
        {
            if (string.IsNullOrWhiteSpace(message) == false)
                await _notificationService.Error(new NotificationConfig
                {
                    Message = message,
                    NotificationType = NotificationType.Error,
                    Placement = NotificationPlacement.TopRight,
                    Duration = 3
                });
        }
    }

    private async Task ShowSuccessMessage(HttpResponseMessage response)
    {
        foreach (var message in (await response.Content.ReadFromJsonAsync<APIResponseModel>()).Messages)
        {
            if (string.IsNullOrWhiteSpace(message) == false)
                await _notificationService.Success(new NotificationConfig
                {
                    Message = message,
                    NotificationType = NotificationType.Success,
                    Placement = NotificationPlacement.TopRight,
                    Duration = 2.5
                });
        }
    }

    private async Task ShowErrorMessage(string message)
    {
        await _notificationService.Error(new NotificationConfig
        {
            Message = message,
            NotificationType = NotificationType.Error,
            Placement = NotificationPlacement.TopRight,
            Duration = 3
        });
    }

    private async Task ShowSuccessMessage(string message)
    {
        await _notificationService.Success(new NotificationConfig
        {
            Message = message,
            NotificationType = NotificationType.Success,
            Placement = NotificationPlacement.TopRight,
            Duration = 2.5
        });
    }
}
