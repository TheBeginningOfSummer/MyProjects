using IPFS.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IPFS;

public class ServiceLocator
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindowVM? MainWindowVM => _serviceProvider.GetService<MainWindowVM>();
    public UploadVM? UploadVM => _serviceProvider.GetService<UploadVM>();
    public DisplayVM? DisplayVM => _serviceProvider.GetService<DisplayVM>();

    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<MainWindowVM>();
        serviceCollection.AddSingleton<UploadVM>();
        serviceCollection.AddSingleton<DisplayVM>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
