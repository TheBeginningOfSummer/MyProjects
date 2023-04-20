using IPFS.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IPFS;

public class ServiceLocator
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindowVM? MainWindowVM => _serviceProvider.GetService<MainWindowVM>();

    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<MainWindowVM>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
