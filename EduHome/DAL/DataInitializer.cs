using EduHome.Constants;
using EduHome.Models;
using Microsoft.AspNetCore.Identity;

namespace EduHome.DAL;

public class DataInitializer
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataInitializer(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }
   
    public async Task SeedData( )
    {
        if (!_context.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin));
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Moderator));
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.User));

        }
    }
}