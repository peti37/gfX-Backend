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

namespace gfX.Controllers
{

    public class HomeController : Controller
    {
        private ICrudRepositories<Models.User> userRepo;

        public HomeController(ICrudRepositories<Models.User> userRepo)
        {
            this.userRepo = userRepo;
        }

        //[AllowAnonymous]
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userRepo.SelectAll();
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

        [HttpGet("signin-github")]
        public async Task<IActionResult> Redirect()
        {
            if (User.Identity.IsAuthenticated)
            {

                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));

                return Ok("OKE");
            }
            return Ok("nemOKE");
        }

    }
}
