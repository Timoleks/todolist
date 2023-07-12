using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodolistApi.Domain.Data;
using TodolistApi.Service.Extensions;

Host
    .CreateDefaultBuilder()
    .ConfigureAppConfiguration(((context, builder) =>
    {
        builder.AddUserSecrets(Assembly.GetExecutingAssembly());
    }))
    .ConfigureServices((context, services) =>
    {
        services.AddTodolistCrudService();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("TodoDatabase")));

        services.AddHostedService<TelegramBot>();

    }).Build().Run();
