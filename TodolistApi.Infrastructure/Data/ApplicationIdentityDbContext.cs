using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodolistApi.Infrastructure.IdentityModels;

namespace TodolistApi.Infrastructure.Data
{
    public class ApplicationIdentityDbContext : IdentityDbContext<User>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
            
        }
    }
}
