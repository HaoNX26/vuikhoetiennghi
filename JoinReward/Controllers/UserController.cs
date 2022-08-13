using JoinReward.Models.DTO;
using JoinReward.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JoinReward.Controllers
{
    public class UserController : Controller
    {
        private readonly Models.DB.UserContext _userContext;
        
        public UserController(Models.DB.UserContext userContext )
        {
            _userContext = userContext;
             
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppUserLogin appUserLogin)
        {
            try
            {
                string s = LogicPie.Security._Hash.PasswordHash.Hash(appUserLogin.PASSWORD);

                if (appUserLogin.USERNAME != "" && appUserLogin.PASSWORD != "")
                {
                    AppUserLogin userModel = await _userContext.LoginByUsernamePasswordMethodAsync(appUserLogin.USERNAME, appUserLogin.PASSWORD);
                    if (userModel != null && userModel.ID > 0)
                    {
                        
                        //last login date
                        var objUser = _userContext.S_USER.Find(userModel.ID);
                        objUser.LASTEST_LOGIN_DATE = DateTime.Now;
                        objUser.NUM_OF_LOGIN_FAILT = 0;
                        _userContext.S_USER.Update(objUser);
                        _userContext.SaveChanges();
                        List<Claim> claims = new List<Claim> {
                            new Claim(ClaimTypes.Name,  (userModel.FULLNAME ==null)?"Không xác định":userModel.FULLNAME),
                            new Claim(ClaimTypes.Email, userModel.USERNAME)
                        };
                        // create identity
                        ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

                        // create principal
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        // sign-in
                        await HttpContext.SignInAsync(
                                scheme: "vuikhoetiennghi",
                                principal: principal,
                                properties: new AuthenticationProperties
                                {
                                    //IsPersistent = true, // for 'remember me' feature
                                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                                });
                        //session
                        HttpContext.Session.SetInt32("s_user_id", (int)userModel.ID);
                        
                        HttpContext.Session.SetString("s_user_name", objUser.USERNAME);
                        HttpContext.Session.SetString("s_fullname", (userModel.FULLNAME == null) ? "Không xác định" : userModel.FULLNAME);
                        HttpContext.Session.SetString("s_role", (objUser.ROLES == null) ? "" : objUser.ROLES);
                        return RedirectToAction("Index", "Admin");

                    }
                    else
                    {
                        ViewData["ERR_MSG"] = (userModel ?? new AppUserLogin()).RESPONSE_MESSAGE;
                        //Khóa tài khoản
                        S_USER s_USER = await _userContext.GetUserByAccountName(appUserLogin.USERNAME);
                        if (s_USER != null)
                        {
                            var objUser = _userContext.S_USER.Find(s_USER.ID);
                            objUser.NUM_OF_LOGIN_FAILT = (objUser.NUM_OF_LOGIN_FAILT == null) ? 0 : objUser.NUM_OF_LOGIN_FAILT + 1;
                            if (objUser.NUM_OF_LOGIN_FAILT >= Constant.C_NUM_FAILT_TO_LOCK_ACCOUNT)
                            {
                                //Khóa tài khoản
                                objUser.IS_EXPIRED = true;

                            }
                            _userContext.S_USER.Update(objUser);
                            _userContext.SaveChanges();
                        }
                    }
                }
                else
                {
                    ViewData["ERR_MSG"] = "Hãy nhập tên đăng nhập và mật khẩu!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["ERR_MSG"] = "Lỗi kết nối với CSDL!" + ex.Message.ToFriendlyErrorMsg();
                return View();
            }
            return View();
        }
         public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(
                   scheme: "vuikhoetiennghi");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
