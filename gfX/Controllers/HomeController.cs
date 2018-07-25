using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using gfX.Models;
using gfX.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using gfX.Services;

namespace gfX.Controllers
{

    public class HomeController : Controller
    {
        private GFXUserService userService;

        public HomeController(GFXUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listOfUsers = await userService.SelectAll();
            bool isNewUser = userService.CheckUser(User.FindFirst(c => c.Type == "urn:github:login")?.Value).Result;
            if (isNewUser)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                await userService.Create(new GFXUser { Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value, GithubHandle = User.FindFirst(c => c.Type == "urn:github:login")?.Value, Repos = userService.EachRepo(accessToken).Result, Email = User.FindFirst(c => c.Type == "urn:github:email")?.Value, Orgs = userService.Orgsozas(accessToken).Result, Avatar = User.FindFirst(c => c.Type == "urn:github:avatar")?.Value });
                listOfUsers = await userService.SelectAll();
                return Ok(listOfUsers);
            }

            return Ok(listOfUsers);
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
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
            var yourProfile = userService.FilterByField(new FilterJson { FieldName = "githubHandle", FieldValue = User.FindFirst(c => c.Type == "urn:github:login")?.Value });
            return Ok(yourProfile);
        }

        [Authorize]
        [HttpGet("profilesettings")]
        public IActionResult ProfileSettings()
        {
            var yourProfile = userService.FilterByField(new FilterJson { FieldName = "githubHandle", FieldValue = User.FindFirst(c => c.Type == "urn:github:login")?.Value });
            bool partOfOrg = yourProfile.Result[0].Orgs.Contains("KoztunkVannak");
            if (!partOfOrg)
            {
                return RedirectToAction("index");
            }
            return Ok(yourProfile);
        }

        [Authorize]
        [HttpPost("profilesettings")]
        public async Task<IActionResult> ProfileSettingsPost([FromBody] FilterJson friss)
        {
            var yourProfile = userService.FilterByField(new FilterJson { FieldName = "githubHandle", FieldValue = User.FindFirst(c => c.Type == "urn:github:login")?.Value });
            await userService.Update(yourProfile.Result[0], friss);
            return RedirectToAction("profilesettings");
        }
    }
}
