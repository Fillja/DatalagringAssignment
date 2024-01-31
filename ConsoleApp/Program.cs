using ConsoleApp.Services;
using Infrastructure.Contexts;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Respositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Programmering\EC\DATALAGRING-COURSE\DatalagringAssignment\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddDbContext<SecondDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Programmering\EC\DATALAGRING-COURSE\DatalagringAssignment\Infrastructure\Data\second_local_database.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<UserRepository>();
    services.AddScoped<AddressRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<VerificationRepository>();
    services.AddScoped<ProfileRepository>();

    services.AddScoped<CategoryRepository>();
    services.AddScoped<ManufacturerRepository>();
    services.AddScoped<ProductRepository>();
    services.AddScoped<OrderRepository>();
    services.AddScoped<OrderRowRepository>();

    services.AddScoped<UserFactories>();
    services.AddScoped<ProductFactories>();

    services.AddScoped<ProductService>();
    services.AddScoped<UserService>();

    services.AddSingleton<MenuService>();
    services.AddSingleton<MainMenuService>();

}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MainMenuService>();
menuService.ShowMainMenu();