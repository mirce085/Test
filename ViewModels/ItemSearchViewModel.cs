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
using System.Windows;
using ConnectionMode.Models;
using Microsoft.Extensions.Configuration;

namespace ConnectionMode.ViewModels;

[INotifyPropertyChanged]
public partial class ItemSearchViewModel : BaseViewModel
{
    [ObservableProperty]
    private int _itemId;

    private string _connectionString;

    public ItemSearchViewModel(IConfiguration config)
    {
        _connectionString = config["ConnectionString"]!;
    }

    [RelayCommand]
    public void SearchItem()
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
            CommandType = CommandType.StoredProcedure,
            CommandText = "GetItemById",
        };


        var parameter = new SqlParameter
        {
            Direction = ParameterDirection.Input,
            SqlDbType = SqlDbType.Int,
            Value = ItemId,
            ParameterName = "@Id"
        };

        command.Parameters.Add(parameter);

        using var reader = command.ExecuteReader();

        if(reader.HasRows)
        {
            while (reader.Read())
            {
                var item = new Item
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Category = reader.GetString("Category"),
                    Description = reader.GetString("Description"),
                    Price = reader.GetInt32("Price"),
                    Quantity = reader.GetInt32("Quantity")
                };

                MessageBox.Show($"Id - {item.Id}\nName - {item.Name}\nCategory - {item.Category}\nDescription - {item.Description}\nPrice - {item.Price}\nQuantity - {item.Quantity}");

            }


        }
        else
        {
            MessageBox.Show("There is no such item!");
        }
    }


    [RelayCommand]
    public void Back()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemControlViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }
}
