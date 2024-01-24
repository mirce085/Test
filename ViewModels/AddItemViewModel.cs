using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CommunityToolkit.Mvvm.Input;
using ConnectionMode.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using ConnectionMode.Models;
using System.IO;
using System.Text.Json;

namespace ConnectionMode.ViewModels;

[INotifyPropertyChanged]
public partial class AddItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _itemName;

    [ObservableProperty]
    private string? _itemDescription;

    [ObservableProperty]
    private string? _itemCategory;

    [ObservableProperty]
    private int _itemAmount;

    [ObservableProperty]
    private int _itemPrice;

    private string _connectionString;

    public AddItemViewModel(IConfiguration config)
    {
        _connectionString = config["ConnectionString"]!;
    }

    [RelayCommand]
    public void AddItem()
    {
        if (ItemCategory == string.Empty || ItemDescription == string.Empty || ItemName == string.Empty)
        {
            return;
        }

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using var command = new SqlCommand
        {
            Connection = connection,
            CommandText = "AddItem",
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.Int,
            Value = ItemPrice,
            ParameterName = "@Price",
        });

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.Int,
            Value = ItemAmount,
            ParameterName = "@Quantity",
        });

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.NVarChar,
            Value = ItemName,
            ParameterName = "@Name",
        });

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.NVarChar,
            Value = ItemDescription,
            ParameterName = "@Description",
        });

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.NVarChar,
            Value = ItemCategory,
            ParameterName = "@Category",
        });

        var output = new SqlParameter
        {
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
            ParameterName = "@ItemId",
        };

        command.Parameters.Add(output);

        var result = command.ExecuteNonQuery();

        var item = new Item
        {
            Id = (int)output.Value,
            Name = ItemName,
            Category = ItemCategory,
            Description = ItemDescription,
            Price = ItemPrice,
            Quantity = ItemAmount
        };

        WeakReferenceMessenger.Default.Send(new AddItemMessage(item));

    }

    [RelayCommand]
    public void Back()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemControlViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
}
