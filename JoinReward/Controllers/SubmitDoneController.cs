using Microsoft.AspNetCore.Mvc;

namespace JoinReward.Controllers
{
    public class SubmitDoneController : Controller
    {
        [Route("guithanhcong")]
        public IActionResult FormSubmitDone()
        {
            return View("FormSubmitDone");
        }

        [Route("guihosothanhcong")]
        public IActionResult FormSubmitCustomerDone()
        {
            return View("SubmitCustomerWinDone");
        }
    }
}
