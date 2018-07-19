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

namespace gfX.Controllers
{
    public class HomeController : Controller
    {
        private ICrudRepositories<User> userRepo;

        public HomeController(ICrudRepositories<User> userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userRepo.SelectAll();
            return Ok(listOfUsers);
        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromBody]User user)
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
        [HttpGet("signin-github")]
        public IActionResult Redirect()
        {
            return Ok("OKE");
        }

    }
}
