using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using gfX.Models;
using gfX.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace gfX.Controllers
{

    public class HomeController : Controller
    {
        private ICrudRepositories<GFXUser> userRepo;

        public HomeController(ICrudRepositories<GFXUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userRepo.SelectAll();
            bool isNewUser = userRepo.CheckUser(User.FindFirst(c => c.Type == "urn:github:login")?.Value).Result;
            if (isNewUser)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                await userRepo.Create(new GFXUser { Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value, GithubHandle = User.FindFirst(c => c.Type == "urn:github:login")?.Value, Repos = userRepo.EachRepo(accessToken).Result, Email = User.FindFirst(c => c.Type == "urn:github:email")?.Value, Orgs = userRepo.Orgsozas(accessToken).Result, Avatar = User.FindFirst(c => c.Type == "urn:github:avatar")?.Value });
                listOfUsers = await userRepo.SelectAll();
                return Ok(listOfUsers);
            }
            
            return Ok(listOfUsers);
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

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return Ok("Ne gyere ide!!!");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var yourProfile = userRepo.FilterByField(new FilterJson {FieldName = "githubHandle", FieldValue = User.FindFirst(c => c.Type == "urn:github:login")?.Value });
            return Json(yourProfile);
        }

    }
}
