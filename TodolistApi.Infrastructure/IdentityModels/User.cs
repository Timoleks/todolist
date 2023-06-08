using Microsoft.AspNetCore.Identity;

namespace TodolistApi.Infrastructure.IdentityModels;

public class User : IdentityUser
{
    public User()
    {
            
    }
    public User(string username) : base(username)
    {
            
    }
}