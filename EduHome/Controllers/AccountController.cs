using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduHome.Constants;
using EduHome.Models;
using EduHome.Services;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P512FiorelloBack.Models;
using EduHome.ViewModels;

namespace EduHome.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;        
        private readonly SignInManager<User> _signInManager;
        private readonly  IMailService _mailService;

        public AccountController(UserManager<User> userManager, IMailService mailService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mailService = mailService;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByNameAsync(model.Login);
            if (user == null) user = await _userManager.FindByEmailAsync(model.Login);

            if(user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View();
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "User is not active");
                return View();
            }


            var signinResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }
            
            return RedirectToAction(nameof(Index), "Home");
            
        }
        

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View();
            }

            User newUser = new User
            {
                FullName = model.Fullname,
                UserName = model.Username,
                Email = model.Email,
                Age = model.Age,
                Position = model.Position,
                
            };
            
           IdentityResult identityResult = await _userManager.CreateAsync(newUser, model.Password);
           if(!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

           await _userManager.AddToRoleAsync(newUser, RoleConstants.User);
           
           var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
           
           var link = Url.Action(nameof(ConfirmEmail), "Account", new { newUser.UserName, token }, Request.Scheme);
           
           await _mailService.SendEmailAsync(new MailRequest
           {
               ToEmail = newUser.Email,
               Subject = "Complete registration",
               Body = link
           });
           
           return RedirectToAction(nameof(Index), "Home");

        }

        public async Task<IActionResult> ConfirmEmail(string username, string token)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();

            var identityResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!identityResult.Succeeded) return BadRequest();
            return RedirectToAction(nameof(Index), "Home");
        }
        
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}