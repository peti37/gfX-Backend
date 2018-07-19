using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gfX.Models;
using gfX.Interfaces;

namespace gfX.Controllers
{
    public class HomeController : Controller
    {
        private ICrudRepositories<User> userRepo;

        public HomeController(ICrudRepositories<User> userRepo)
        {
            this.userRepo = userRepo;
        }

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

        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody]FilterJson json)
        {
            var filteredUsers = await userRepo.FilterByField(json);
            return Ok(filteredUsers);
        }

    }
}
