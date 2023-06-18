using Microsoft.AspNetCore.Identity;
using System;

namespace TodolistApi.Service.HostedServices
{
	public class IdentityRolesInitializer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IdentityRolesInitializer> _logger;

        public IdentityRolesInitializer(ILogger<IdentityRolesInitializer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (await roleManager.RoleExistsAsync("ADMIN"))
                {
                    _logger.LogInformation("Role already exists");
                    return;
                }

                var result = await roleManager.CreateAsync(new IdentityRole("ADMIN"));

                
                    if (result.Succeeded)
                {
                    _logger.LogInformation("Role admin created");
                    return;
                }
                
                _logger.LogInformation("Role didnt create");
            }

            

            
        }
    }
}

