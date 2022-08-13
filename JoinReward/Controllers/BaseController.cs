using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
 
using Microsoft.AspNetCore.Mvc.Filters;

namespace JoinReward.Controllers
{
    public class BaseController : Controller
    {
        protected BaseController()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Nếu phải thay đổi mật khẩu. Sang trang thay đổi
            HttpContext.Session.SetString("s_has_to_change_pass", "Y");
            string v_has_to_change_pass = (HttpContext.Session.GetString("s_has_to_change_pass") != null) ? "Y" : "N";
            
            string s_role = HttpContext.Session.GetString("s_role");
            string s_fullname = HttpContext.Session.GetString("s_fullname");
            ViewBag.Role = s_role;
            ViewBag.FullName = s_fullname;
            
            if (s_role != null)
            {
                 
            }
            else
            {

                filterContext.Result = new LocalRedirectResult("/User/Login");
            }
        }
 

    }
}