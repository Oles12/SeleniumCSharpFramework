using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Extensions.DependencyInjection;

namespace EaApplicationTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IDriverFixture, DriverFixture>()
            .AddScoped<IDriverWait, DriverWait>()         
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IProductPage, ProductPage>();
    }                
}                  