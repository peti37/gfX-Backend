using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gfX.Models;
using gfX.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Octokit;
using Octokit.Internal;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Security.Claims;
using Octokit;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace gfX.Controllers
{

    public class HomeController : Controller
    {
        private ICrudRepositories<Models.Userke> userRepo;

        public HomeController(ICrudRepositories<Models.Userke> userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok("Home page for " + User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value);
            }
            var listOfUsers = await userRepo.SelectAll();
            return Ok(listOfUsers);
        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromBody]Models.Userke user)
        {
            await userRepo.Create(user);
            return RedirectToAction("Index");   
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet("signin-github")]
        public IActionResult Redirect()
        {

            if (User.Identity.IsAuthenticated)
            {
                return Ok("Home page for " + User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value + ClaimTypes.Name);
            }
            else
            {
                return Ok("Home page for guest user." + ClaimTypes.Name);
            }
        }   
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody]FilterJson json)
        {
            var filteredUsers = await userRepo.FilterByField(json);
            return Ok(filteredUsers);

        }

    }
}
