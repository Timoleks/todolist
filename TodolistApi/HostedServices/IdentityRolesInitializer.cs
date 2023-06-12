using System;

namespace TodolistApi.Service.HostedServices
{
	public class IdentityRolesInitializer : BackgroundService
	{
        private readonly ILogger<IdentityRolesInitializer> _logger;

        public IdentityRolesInitializer(ILogger<IdentityRolesInitializer> logger)
        {
            _logger = logger;
            
        }

        

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogError("start");
            return Task.CompletedTask;
        }
    }
}

