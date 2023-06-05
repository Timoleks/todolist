using Microsoft.Extensions.DependencyInjection;
using TodolistApi.Domain.Models;
using TodolistApi.Service.Repository;

namespace TodolistApi.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTodolistCrudService(this IServiceCollection services) =>
            services.AddScoped<IRepository<TodoItem>, Repository<TodoItem>>();
    }
}
