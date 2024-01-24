using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ConnectionMode.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionMode.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<ChangeViewModelMessage>(this, (sender, message) =>
        {
            CurrentViewModel = message.ViewModel;
        });

        CurrentViewModel = App.ServiceProvider.GetRequiredService<ItemControlViewModel>();
    }

    [ObservableProperty]
    public BaseViewModel _currentViewModel = null!;
}
