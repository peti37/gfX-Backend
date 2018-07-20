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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using gfX.Repositories;

namespace gfX.Controllers
{

    public class HomeController : Controller
    {
        private ICrudRepositories<Models.User> userRepo;

        public HomeController(ICrudRepositories<Models.User> userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userRepo.SelectAll();
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                //var github = new GitHubClient(new ProductHeaderValue("fasz"), new InMemoryCredentialStore(new Credentials(accessToken)));
                //var Repositories = await github.Repository.GetAllForCurrent();
                
                await userRepo.Create(new Models.User { Name = "pityu", Repos = userRepo.EachRepo(accessToken).Result });
                return Ok(listOfUsers);
            }
            return Ok(listOfUsers);
        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromBody]Models.User user)
        {
            await userRepo.Create(user);
            return RedirectToAction("Index");   
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("GitHub", new AuthenticationProperties
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in the 
                // **Allowed Logout URLs** settings for the client.
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
