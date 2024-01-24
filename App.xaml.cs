using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ConnectionMode.ViewModels;
using ConnectionMode.Views;
using Microsoft.Extensions.Configuration;

namespace ConnectionMode;


public partial class App : Application
{
    public static ServiceCollection ServiceCollection { get; private set; } = null!;
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceCollection = new ServiceCollection();

        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile("config.json");

        IConfiguration configuration = configurationBuilder.Build();

        ServiceCollection.AddSingleton<IConfiguration>(provider =>
        {
            return configuration;
        });

        ServiceCollection.AddSingleton<MainViewModel>();
        ServiceCollection.AddSingleton<MainView>();
        ServiceCollection.AddSingleton<ItemControlViewModel>();
        ServiceCollection.AddSingleton<ItemListViewModel>();
        ServiceCollection.AddTransient<AddItemViewModel>();
        ServiceCollection.AddTransient<DeleteItemViewModel>();
        ServiceCollection.AddTransient<UpdateItemViewModel>();
        ServiceCollection.AddTransient<ItemSearchViewModel>();

        ServiceProvider = ServiceCollection.BuildServiceProvider();

        var view = ServiceProvider.GetService<MainView>()!;

        view.Show();
    }
}
