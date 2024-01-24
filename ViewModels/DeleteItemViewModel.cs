using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectionMode.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ConnectionMode.ViewModels;

[INotifyPropertyChanged]
public partial class DeleteItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private int _itemId;

    private string _connectionString;

    public DeleteItemViewModel(IConfiguration config)
    {
        _connectionString = config["ConnectionString"]!;
    }

    [RelayCommand]
    public void DeleteItem()
    {
        if(ItemId == 0)
        {
            return;
        }

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using var command = new SqlCommand
        {
            Connection = connection,
            CommandText = "DeleteItemById",
            CommandType = System.Data.CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter
        {
            Direction = System.Data.ParameterDirection.Input,
            SqlDbType = System.Data.SqlDbType.Int,
            ParameterName = "@Id",
            Value = ItemId,
        });

        using var reader = command.ExecuteReader();
    }

    [RelayCommand]
    public void Back()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemControlViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
}
