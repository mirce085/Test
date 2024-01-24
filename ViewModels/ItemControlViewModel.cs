using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectionMode.Messages;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace ConnectionMode.ViewModels;

public partial class ItemControlViewModel : BaseViewModel
{
    [RelayCommand]
    public void OpenAddItemView()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<AddItemViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    public void OpenDeleteItemView()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<DeleteItemViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    public void OpenUpdateItemView()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<UpdateItemViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
    
    [RelayCommand]
    public void OpenItemListView()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemListViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
    
    [RelayCommand]
    public void OpenItemSearchView()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemSearchViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
}
