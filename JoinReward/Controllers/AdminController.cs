using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace JoinReward.Controllers
{
    //[Authorize]
    public class AdminController : BaseController
    {
        [Route("admin")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            return View("Admin");
        }
    }
}
