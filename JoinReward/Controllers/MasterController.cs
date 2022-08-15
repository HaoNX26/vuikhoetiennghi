using JoinReward.Models;
using JoinReward.Models.DTO;
using JoinReward.Models.Master;
using JoinReward.Models.SubmitCustomerWin;
using JoinReward.Models.User;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Text;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace JoinReward.Controllers
{
    public class MasterController : BaseController
    {

        private readonly Models.DB.MasterContext _masterContext;
        private readonly Models.DB.BusinessContext _submitCustomerWinContext;
        private readonly Models.DB.UserContext _userContext;

        public MasterController(Models.DB.MasterContext masterContext, Models.DB.BusinessContext submitCustomerWinContext, Models.DB.UserContext userContext)
        {
            _masterContext = masterContext;
            _submitCustomerWinContext = submitCustomerWinContext;
            _userContext = userContext;
        }
        public IActionResult ASCMaster(ASCMasterDTO aSCMasterDTO)
        {
            try
            {

                string s_role = HttpContext.Session.GetString("s_role");

                if (s_role != SysFrameworks.Constant.C_ROLE_ADMIN && s_role != SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    if (aSCMasterDTO.PAGE_SIZE <= 0) aSCMasterDTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                    if (aSCMasterDTO.page <= 0) aSCMasterDTO.page = 1;
                    Models.Pagination pagination = new Models.Pagination()
                    {
                        PAGE_SIZE = aSCMasterDTO.PAGE_SIZE,
                        CUR_PAGE = aSCMasterDTO.page
                    };
                    List<ASCMasterDTO> ASCMasterDTOs = _masterContext.SearchAsc(aSCMasterDTO.KEYWORD, pagination.CUR_PAGE, pagination.PAGE_SIZE);
                    if (ASCMasterDTOs.Count > 0) pagination.TOTAL_RECORD = ASCMasterDTOs[0].NUM_OF_RECORD;
                    pagination.CONTROLLER = ControllerContext.ActionDescriptor.ControllerName;
                    pagination.ACTION = ControllerContext.ActionDescriptor.ActionName;
                    //L?y thu?c tính truy?n qua query string
                    DataCollections cols = new DataCollections();
                    cols.Add(DataTypes.NVarchar, "keyword", FieldTypes.DefaultValue, aSCMasterDTO.KEYWORD, "");
                    pagination.GET_QUERY_STRING = SysFrameworks.Url.to_Get_Param(cols);
                    ViewData["pagination"] = pagination;
                    ViewData["grid_data"] = ASCMasterDTOs;
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message.ToString();
            }
            return View("ASCMaster");
        }

        public IActionResult ShowASCMasterEdit(long id)
        {
            string s_role = HttpContext.Session.GetString("s_role");

            if (s_role != SysFrameworks.Constant.C_ROLE_ADMIN && s_role != SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "User");
            }
            ASCMaster aSCMaster = new ASCMaster();
            ASCMasterDTO aSCMasterDTO = new ASCMasterDTO();
            if (id > 0)
            {
                aSCMaster = _masterContext.aSCMasters.Find(id);
                S_USER user = _userContext.S_USER.Where(p => p.ASC_ID == id).FirstOrDefault<S_USER>();
                aSCMasterDTO.USERNAME = user.USERNAME;
                aSCMasterDTO.ASC_CODE = aSCMaster.ASC_CODE;
                aSCMasterDTO.ASC_EMAIL = aSCMaster.ASC_EMAIL;
                aSCMasterDTO.ASC_NAME = aSCMaster.ASC_NAME;
            }
            return PartialView("ASCMasterEdit", aSCMasterDTO);
        }

        public JsonResult SaveASC(ASCMasterDTO aSCMaster)
        {
            JsonResponse response = new JsonResponse();
            if (SysFrameworks.CString.IsValidEmail(aSCMaster.ASC_EMAIL))
            {
                using (var dbContextTransaction = _masterContext.Database.BeginTransaction())
                {
                    try
                    {
                        string password = CString.GetRandomPassword();
                        string srrMsg = "";
                        if (srrMsg.Length <= 0)
                        {
                            if (aSCMaster.ID <= 0)
                            {
                                ASCMaster sCMasterInsert = new ASCMaster();
                                sCMasterInsert.ASC_CODE = aSCMaster.ASC_CODE;
                                sCMasterInsert.ASC_EMAIL = aSCMaster.ASC_EMAIL;
                                sCMasterInsert.ASC_NAME = aSCMaster.ASC_NAME;
                                sCMasterInsert.ASC_TYPE = SysFrameworks.Constant.C_ROLE_ASC;
                                sCMasterInsert.CREATE_DATE = DateTime.Now;
                                sCMasterInsert.CREATED_BY = (long)HttpContext.Session.GetInt32("s_user_id").Value;
                                _masterContext.aSCMasters.Add(sCMasterInsert);
                                _masterContext.SaveChanges();
                                S_USER user = new S_USER();
                                user.ASC_ID = sCMasterInsert.ID;
                                user.PASSWORD = LogicPie.Security._Hash.PasswordHash.Hash(password);
                                user.USERNAME = aSCMaster.ASC_CODE;
                                user.ROLES = SysFrameworks.Constant.C_ROLE_ASC;
                                user.FULLNAME = aSCMaster.ASC_NAME;
                                user.NUM_OF_LOGIN_FAILT = 0;
                                user.CREATE_DATE = DateTime.Now;
                                user.CREATED_BY = (int)HttpContext.Session.GetInt32("s_user_id").Value;
                                _masterContext.S_USER.Add(user);

                                if (SendEmail(aSCMaster.ASC_EMAIL, aSCMaster.ASC_NAME, aSCMaster.ASC_CODE, password))
                                {
                                    _masterContext.SaveChanges();
                                }
                                else
                                {
                                    response.success = "N";
                                    response.message ="Lỗi gửi email tài khoản!";
                                }
                              
                            }
                            else
                            {
                                ASCMaster sCMasterUpdate = _masterContext.aSCMasters.Find(aSCMaster.ID);
                                sCMasterUpdate.ASC_CODE = aSCMaster.ASC_CODE;
                                sCMasterUpdate.ASC_EMAIL = aSCMaster.ASC_EMAIL;
                                sCMasterUpdate.ASC_NAME = aSCMaster.ASC_NAME;
                                sCMasterUpdate.MODIFY_DATE = DateTime.Now;
                                sCMasterUpdate.MODIFIED_BY = (long)HttpContext.Session.GetInt32("s_user_id").Value;
                                _masterContext.aSCMasters.Update(sCMasterUpdate);

                                _masterContext.SaveChanges();
                            }
                            //MailService.sendMail(aSCMaster.ASC_EMAIL.Trim(), password, aSCMaster.USERNAME);
                            dbContextTransaction.Commit();

                        }
                        else
                        {
                            response.success = "N";
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        response.success = "N";
                        response.message = SysFrameworks.MessageLib.ToFriendlyMessage(ex.InnerException.ToString());
                    }
                }
            }
            else
            {
                response.success = "N";
                response.message = "Email không hợp lệ!";
            }
           
           
            return Json(response);
        }
        private bool SendEmail(string to_email, string asc_name, string user_name, string password)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("support@vuikhoetiennghi.vn"));
                email.To.Add(MailboxAddress.Parse(to_email));
                email.Subject = "Tài khoản sử dụng hệ thống vuikhoetiennghi.vn - ASC: " + asc_name;
                email.Body = new TextPart(TextFormat.Html) {
                    Text = "<h1>Thông tin truy cập hệ thống là:</h1>"
                    + "<br>"
                    + "<div> Tài khoản: " + user_name + "</div>"
                    + "<div> Mật khẩu: " + password + "</div>"
                    + "<div> Link truy cập: https://vuikhoetiennghi.vn/admin </div>"
                    + "<br> <br>"
                     + "Vui lòng không phản hồi lại email này!"
                };

                // send email
                var smtp = new SmtpClient();
                smtp.Connect("pro17.emailserver.vn", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("support@vuikhoetiennghi.vn", "q7kJ5Eld(EvL");
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public JsonResult DeleteASC(long id)
        {
            JsonResponse response = new JsonResponse();

            //Check xem da su dung du lieu hay chua
            List<SubmitInfoCustomerWin> submitInfoCustomerWins = _masterContext.SubmitInfoCustomerWin.Where(p => p.AscId == id).ToList<SubmitInfoCustomerWin>();
            if(submitInfoCustomerWins != null && submitInfoCustomerWins.Count> 0)
            {
                response.success = "N";
                response.message ="Dữ liệu đã được sử dụng. Không thể xóa!";
            }
            else
            {
                using (var dbContextTransaction = _masterContext.Database.BeginTransaction())
                {
                    try
                    {
                        S_USER s_USER = _masterContext.S_USER.Where(p => p.ASC_ID.Equals(id)).FirstOrDefault<S_USER>();
                        if (s_USER != null)
                        {


                            _masterContext.S_USER.Remove(s_USER);
                        }
                        _masterContext.SaveChanges();

                        ASCMaster aSCMaster = _masterContext.aSCMasters.Find(id);
                        _masterContext.aSCMasters.Remove(aSCMaster);
                        _masterContext.SaveChanges();
                        _masterContext.Database.CommitTransaction();
                        
                    }
                    catch (Exception ex)
                    {
                             
                        _masterContext.Database.RollbackTransaction();
                        response.success = "N";
                        response.message = ex.Message;
                    }
                    
                }
            }
         
            return Json(response);
        }
        public IActionResult MakeOTP(M_OTP_DTO m_OTP_DTO)
        {
            try
            {

                string s_role = HttpContext.Session.GetString("s_role");

                if (s_role != SysFrameworks.Constant.C_ROLE_ADMIN && s_role != SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    if (m_OTP_DTO.PAGE_SIZE <= 0) m_OTP_DTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                    if (m_OTP_DTO.page <= 0) m_OTP_DTO.page = 1;
                    Models.Pagination pagination = new Models.Pagination()
                    {
                        PAGE_SIZE = m_OTP_DTO.PAGE_SIZE,
                        CUR_PAGE = m_OTP_DTO.page
                    };
                    List<M_OTP_DTO> m_OTP_DTOs   = _masterContext.SearchOTP(m_OTP_DTO.KEYWORD, pagination.CUR_PAGE, pagination.PAGE_SIZE);
                    if (m_OTP_DTOs.Count > 0) pagination.TOTAL_RECORD = m_OTP_DTOs[0].NUM_OF_RECORD;
                    pagination.CONTROLLER = ControllerContext.ActionDescriptor.ControllerName;
                    pagination.ACTION = ControllerContext.ActionDescriptor.ActionName;
                    //L?y thu?c tính truy?n qua query string
                    DataCollections cols = new DataCollections();
                    cols.Add(DataTypes.NVarchar, "keyword", FieldTypes.DefaultValue, m_OTP_DTO.KEYWORD, "");
                    pagination.GET_QUERY_STRING = SysFrameworks.Url.to_Get_Param(cols);
                    ViewData["pagination"] = pagination;
                    ViewData["grid_data"] = m_OTP_DTOs;
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message.ToString();
            }
            return View("OTP");
        }
        public JsonResult DeleteOTP(long id)
        {
            JsonResponse response = new JsonResponse();

            try
            {
                M_OTP m_OTP   = _masterContext.M_OTP.Where(p => p.ID == id ).FirstOrDefault<M_OTP>();
                if (m_OTP != null)
                {


                    _masterContext.M_OTP.Remove(m_OTP);
                }
                _masterContext.SaveChanges();
 
            }
            catch (Exception ex)
            {

               
                response.success = "N";
                response.message = ex.Message;
            }

            return Json(response);
        }
        public IActionResult ShowOTPEdit(long id)
        {
            string s_role = HttpContext.Session.GetString("s_role");

            if (s_role != SysFrameworks.Constant.C_ROLE_ADMIN && s_role != SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "User");
            }
            M_OTP   m_OTP = new M_OTP();
            ASCMasterDTO aSCMasterDTO = new ASCMasterDTO();
            if (id > 0)
            {
                m_OTP = _masterContext.M_OTP.Find(id);
                 
              
            }
            return PartialView("OTPEdit", m_OTP);
        }
        public JsonResult SaveOTP(M_OTP m_OTP )
        {
            JsonResponse response = new JsonResponse();
            if (m_OTP.ID <= 0)
            {

                M_OTP optInsert = new M_OTP();
                optInsert.OTP = m_OTP.OTP;
                optInsert.CUSTOMER_TEL = m_OTP.CUSTOMER_TEL.ToString().ToNormalPhoneNumber();
                _masterContext.M_OTP.Add(optInsert);
                optInsert.CREATE_DATE = DateTime.Now;
                optInsert.CREATED_BY = (long)HttpContext.Session.GetInt32("s_user_id").Value;
                _masterContext.SaveChanges();
              
            }
            else
            {
                M_OTP optUpdate = _masterContext.M_OTP.Find(m_OTP.ID);
                optUpdate.OTP = m_OTP.OTP;
                optUpdate.CUSTOMER_TEL = m_OTP.CUSTOMER_TEL.ToString().ToNormalPhoneNumber();
                optUpdate.MODIFY_DATE = DateTime.Now;
                optUpdate.MODIFIED_BY = (long)HttpContext.Session.GetInt32("s_user_id").Value;
                _masterContext.M_OTP.Update(optUpdate);
                _masterContext.SaveChanges();
            }
           
            return Json(response);
        }
    }
}
