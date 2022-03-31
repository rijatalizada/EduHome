using EduHome.Areas.Dashboard.ViewModels;
using EduHome.Constants;
using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers;
[Area("Dashboard")]
[Authorize(Roles = RoleConstants.Admin )]
public class UserController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }


    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        
        List<UserViewModel> userViewList = new List<UserViewModel>();

        foreach (var user in users)
        {
            userViewList.Add(new UserViewModel
            {
                Id = user.Id,
                Fullname = user.FullName,
                Username = user.UserName,
                Roles =  string.Join(",",await _userManager.GetRolesAsync(user)),
                IsActive = user.IsActive
            });
        }
        return View(userViewList);
    }

    public async Task<IActionResult> GetRoles(string id)
    {
        var users = await _userManager.FindByIdAsync(id);
        if (!ModelState.IsValid) return NotFound();
            
        var roles = await _userManager.GetRolesAsync(users);
        ViewBag.Username = users.UserName;
        ViewBag.Id = users.Id;
        return View(roles);
    }
    
    public async Task<IActionResult> RemoveRole(string id, string roleName)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        
        await _userManager.RemoveFromRoleAsync(user, roleName);
        return RedirectToAction(nameof(GetRoles), new
        {
            user.Id
        });
    }
    
    public async Task<IActionResult> AddRole(string id)
    {
        var roles = await _context.Roles.Select(r => r.Name).ToListAsync();

        AddRolesViewModel model = new AddRolesViewModel()
        {
            UserId = id,
            Roles = roles
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(string id, AddRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        model.Roles = await _context.Roles.Select(r => r.Name).ToListAsync();
        model.UserId = id;
        if (!ModelState.IsValid) return View(model);

        await _userManager.AddToRoleAsync(user, model.RoleName);

        return RedirectToAction(nameof(GetRoles), new
        {
            id
        });

    }
    
    public async Task<IActionResult> ChangePassword(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        ViewBag.Username = user.UserName;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!ModelState.IsValid) return View();

        if(!await _userManager.CheckPasswordAsync(user, model.OldPassword))
        {
            ModelState.AddModelError(nameof(ChangePasswordViewModel.OldPassword), "Old Password is not correct");
            return View();
        }

        var idResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (!idResult.Succeeded)
        {
            foreach (var error in idResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> ToggleBlock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.IsActive = !user.IsActive;
        
        await _userManager.UpdateAsync(user);   
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}