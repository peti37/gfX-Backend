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
        private ICrudRepositories<Models.GFXUser> userRepo;

        public HomeController(ICrudRepositories<Models.GFXUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userRepo.SelectAll();

            if (userRepo.CheckUser(User.FindFirst(c => c.Type == "urn:github:login")?.Value).Result)
            {
                await userRepo.Create(new GFXUser { Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value, GithubHandle = User.FindFirst(c => c.Type == "urn:github:login")?.Value });
                listOfUsers = await userRepo.SelectAll();
                return Ok(listOfUsers);
            }
            
            return Ok(listOfUsers);
        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromBody]GFXUser user)
        {
            await userRepo.Create(user);
            return RedirectToAction("Index");   
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
        
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody]FilterJson json)
        {
            var filteredUsers = await userRepo.FilterByField(json);
            return Ok(filteredUsers);

        }

    }
}
