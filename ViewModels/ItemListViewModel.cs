using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using ConnectionMode.Models;
using ConnectionMode.Messages;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;


namespace ConnectionMode.ViewModels;

public partial class ItemListViewModel : BaseViewModel
{
    public string AmountStr { get; set; }
    public ObservableCollection<Item> ItemList {  get; private set; }

    private string _connectionString;

    public ItemListViewModel(IConfiguration config)
    {
        _connectionString = config["ConnectionString"]!;

        ItemList = new ObservableCollection<Item>();

        WeakReferenceMessenger.Default.Register<AddItemMessage>(this, (sender, e) =>
        {
            ItemList.Add(e.Item);
            AmountStr = $"Item Amount: {ItemList.Count}";

            SaveItems();
        });

        WeakReferenceMessenger.Default.Register<UpdateItemMessage>(this, (sender, e) =>
        {
            ItemList.Remove(ItemList.FirstOrDefault(x => x.Id == e.Item.Id)!);
            ItemList.Add(e.Item);
            ItemList.OrderBy(x => x.Id);

            SaveItems();
        });

        using var connection = new SqlConnection(_connectionString);

        connection.Open();

        using var command = new SqlCommand
        {
            Connection = connection,
            CommandType = CommandType.StoredProcedure,
            CommandText = "GetAllItems",
        };

        using var reader = command.ExecuteReader();


        if (reader.HasRows)
        {
            while (reader.Read())
            {
                ItemList.Add(new Item
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Category = reader.GetString("Category"),
                    Description = reader.GetString("Description"),
                    Price = reader.GetInt32("Price"),
                    Quantity = reader.GetInt32("Quantity"),
                });
            }
        }
        AmountStr = $"Item Amount: {ItemList.Count}";
        
    }

    private void SaveItems()
    {
        var writer = new StreamWriter("data.json");
        var json = JsonSerializer.Serialize(ItemList);
        writer.Write(json);
    }

    [RelayCommand]
    public void Back()
    {
        var message = new ChangeViewModelMessage(App.ServiceProvider.GetRequiredService<ItemControlViewModel>());

        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    public void ShowItemDescription(Item item)
    {
        MessageBox.Show($"Description:\n{item.Description}");
    }
}
