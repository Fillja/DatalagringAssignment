using ConsoleApp;
using Infrastructure.Contexts;
using Infrastructure.Factories;
using Infrastructure.Respositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Programmering\EC\DATALAGRING-COURSE\DataStoringAssignment\Shared\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"));

    services.AddScoped<UserRepository>();
    services.AddScoped<AddressRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<VerificationRepository>();
    services.AddScoped<ProfileRepository>();
    services.AddScoped<UserFactories>();

    services.AddScoped<UserService>();
    services.AddScoped<MenuService>();

}).Build();

builder.Start();

var menuService = builder.Services.GetRequiredService<MenuService>();
menuService.ShowUserMenu();