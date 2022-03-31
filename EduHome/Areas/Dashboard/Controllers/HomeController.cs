using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduHome.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        [Area("Dashboard")]
        [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]
        public IActionResult Index()
        {
            return View();
        }
    }
}