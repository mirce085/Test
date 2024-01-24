using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ConnectionMode.Messages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionMode.Models;
using Microsoft.Extensions.Configuration;

namespace ConnectionMode.ViewModels;

[INotifyPropertyChanged]
public partial class UpdateItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private int _itemId;

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

    public UpdateItemViewModel(IConfiguration config)
    {
        _connectionString = config["ConnectionString"]!;
    }


    [RelayCommand]
    public void UpdateItem()
    {
        if (ItemCategory == string.Empty || ItemDescription == string.Empty || ItemName == string.Empty || ItemId == 0)
        {
            return;
        }

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using var command = new SqlCommand
        {
            Connection = connection,
            CommandText = "UpdateItemById",
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter
        {
            Direction = ParameterDirection.Input,
            ParameterName = "@Id",
            Value = ItemId,
            SqlDbType = SqlDbType.Int,
        });

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

        var result = command.ExecuteNonQuery();

        if (result != 0)
        {
            var item = new Item
            {
                Id = ItemId,
                Name = ItemName,
                Category = ItemCategory,
                Description = ItemDescription,
                Price = ItemPrice,
                Quantity = ItemAmount,
            };

            WeakReferenceMessenger.Default.Send(new UpdateItemMessage(item));

        }
    }


    [RelayCommand]
    public void Back()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemControlViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }


}
