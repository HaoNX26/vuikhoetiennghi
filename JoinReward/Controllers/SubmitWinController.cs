﻿using JoinReward.Models;
using JoinReward.Models.DB;
using JoinReward.Models.DTO;
using JoinReward.Models.Log;
using JoinReward.Models.Master;
using JoinReward.Models.SubmitCustomerWin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JoinReward.Controllers
{
    public class SubmitWinController : Controller
    {

        private readonly ILogger<SubmitWinController> _logger;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly AppRegisterContext _appRegisterContext;
        private readonly SubmitCustomerWinContext _submitCustomerWinContext;
        private readonly MasterContext _masterContext;

        public SubmitWinController(ILogger<SubmitWinController> logger, IHostingEnvironment hostingEnvironment, AppRegisterContext appRegisterContext, SubmitCustomerWinContext submitCustomerWinContext, MasterContext masterContext)
        {
            _logger = logger;
            _hostEnvironment = hostingEnvironment;
            _appRegisterContext = appRegisterContext;
            _submitCustomerWinContext = submitCustomerWinContext;
            _masterContext = masterContext;
        }

        public JsonResult ReceiveOtp(string PhoneNumber)
        {

            JsonResponse responseJson = new JsonResponse();
            try
            {
                if (PhoneNumber != null || PhoneNumber != "")
                {

                    //if (PhoneNumber.Equals("0968197685") || PhoneNumber.Equals("84355990323"))
                    //{
                    //    responseJson.success = "Y";
                    //    return Json(responseJson);
                    //}

                    //BCustomerWin bCustomerWin = _submitCustomerWinContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()))).FirstOrDefault<BCustomerWin>();
                    B_INPUT_PHONE_LOG b_INPUT_PHONE_LOG = new B_INPUT_PHONE_LOG();
                    b_INPUT_PHONE_LOG.PHONE_NUMBER = PhoneNumber.Trim();
                    b_INPUT_PHONE_LOG.CREATED_DATE = DateTime.Now;
                    b_INPUT_PHONE_LOG.NOTE = "RECEIVE";
                    _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(b_INPUT_PHONE_LOG);
                    _submitCustomerWinContext.SaveChanges();
                    //if (bCustomerWin != null)
                    //{
                    string urlParameters = "?phoneNumber=" + PhoneNumber.Trim() + "&secret_key=235klkl6353Qfersd";
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Constant.URL_RECEIVE_OTP);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    // List data response.
                    HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        B_INPUT_PHONE_LOG B_INPUT_PHONE_LOG_SUCCESS = new B_INPUT_PHONE_LOG();
                        B_INPUT_PHONE_LOG_SUCCESS.PHONE_NUMBER = PhoneNumber.Trim();
                        B_INPUT_PHONE_LOG_SUCCESS.CREATED_DATE = DateTime.Now;
                        B_INPUT_PHONE_LOG_SUCCESS.NOTE = "SUCCESS";
                        _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(B_INPUT_PHONE_LOG_SUCCESS);
                        _submitCustomerWinContext.SaveChanges();

                        responseJson.success = "Y";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        responseJson.success = "N";
                        var deserialized = JsonConvert.DeserializeObject<JsonResponse>(response.Content.ReadAsStringAsync().Result);
                        responseJson.message = deserialized.message;

                        B_INPUT_PHONE_LOG B_INPUT_PHONE_LOG_FAIL = new B_INPUT_PHONE_LOG();
                        B_INPUT_PHONE_LOG_FAIL.PHONE_NUMBER = PhoneNumber.Trim();
                        B_INPUT_PHONE_LOG_FAIL.CREATED_DATE = DateTime.Now;
                        B_INPUT_PHONE_LOG_FAIL.NOTE = "FAIL_" + deserialized.message;
                        _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(B_INPUT_PHONE_LOG_FAIL);
                        _submitCustomerWinContext.SaveChanges();
                        //responseJson.message = "Số điện thoại không hợp lệ hoặc không nằm trong danh sách trúng thưởng";
                    }
                    else
                    {
                        responseJson.success = "N";
                        responseJson.message = "Có lỗi xảy ra. Liên hệ với hotline Panasonic!";
                    }

                    //Make any other calls using HttpClient here.

                    //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                    client.Dispose();
                    //}
                    //else
                    //{
                    //    responseJson.success = "N";
                    //    responseJson.message = "Không thấy số điện thoại trong danh sách trúng thưởng. Liên hệ với hotline Panasonic!";
                    //}
                }
                else
                {
                    responseJson.success = "N";
                    responseJson.message = "Chưa nhập số điện thoại";
                }
            }
            catch (Exception ex)
            {

                responseJson.success = "N";
                responseJson.message = ex.Message;
            }
            return Json(responseJson);
        }

        public JsonResult VerifyOtp(string PhoneNumber, string otp)
        {

            JsonResponse responseJson = new JsonResponse();
            try
            {
                if (PhoneNumber != null || PhoneNumber != "")
                {

                    //if (PhoneNumber.Equals("0968197685") || PhoneNumber.Equals("84355990323"))
                    //{
                    //    HttpContext.Session.SetString("PhoneNumber", SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()));
                    //    responseJson.success = "Y";
                    //    return Json(responseJson);
                    //}

                    //Kiem tra xem co trong bang OTP local khong
                    M_OTP m_OTP = _masterContext.M_OTP.Where(p => p.CUSTOMER_TEL == PhoneNumber.ToNormalPhoneNumber() && p.OTP == otp).FirstOrDefault<M_OTP>();
                    if (m_OTP != null)

                    {
                        HttpContext.Session.SetString("PhoneNumber", SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()));
                        responseJson.success = "Y";
                    }
                    else
                    {
                        B_INPUT_PHONE_LOG B_INPUT_PHONE_LOG_VERIFY = new B_INPUT_PHONE_LOG();
                        B_INPUT_PHONE_LOG_VERIFY.PHONE_NUMBER = PhoneNumber.Trim();
                        B_INPUT_PHONE_LOG_VERIFY.CREATED_DATE = DateTime.Now;
                        B_INPUT_PHONE_LOG_VERIFY.NOTE = "VERIFY";
                        _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(B_INPUT_PHONE_LOG_VERIFY);
                        _submitCustomerWinContext.SaveChanges();

                        string urlParameters = "?phoneNumber=" + PhoneNumber.Trim() + "&otp=" + otp.Trim();
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Constant.URL_VERIFY_OTP);
                        // Add an Accept header for JSON format.
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                        // List data response.
                        HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            B_INPUT_PHONE_LOG B_INPUT_PHONE_LOG_SUCCESS = new B_INPUT_PHONE_LOG();
                            B_INPUT_PHONE_LOG_SUCCESS.PHONE_NUMBER = PhoneNumber.Trim();
                            B_INPUT_PHONE_LOG_SUCCESS.CREATED_DATE = DateTime.Now;
                            B_INPUT_PHONE_LOG_SUCCESS.NOTE = "VERIFY SUCCESS";
                            _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(B_INPUT_PHONE_LOG_SUCCESS);
                            _submitCustomerWinContext.SaveChanges();

                            HttpContext.Session.SetString("PhoneNumber", SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()));
                            responseJson.success = "Y";
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            var deserialized = JsonConvert.DeserializeObject<JsonResponse>(response.Content.ReadAsStringAsync().Result);

                            B_INPUT_PHONE_LOG B_INPUT_PHONE_LOG_FAIL = new B_INPUT_PHONE_LOG();
                            B_INPUT_PHONE_LOG_FAIL.PHONE_NUMBER = PhoneNumber.Trim();
                            B_INPUT_PHONE_LOG_FAIL.CREATED_DATE = DateTime.Now;
                            B_INPUT_PHONE_LOG_FAIL.NOTE = "VERIFY FAIL_" + deserialized.message;
                            _submitCustomerWinContext.B_INPUT_PHONE_LOG.Add(B_INPUT_PHONE_LOG_FAIL);
                            _submitCustomerWinContext.SaveChanges();

                            responseJson.success = "N";
                            responseJson.message = "Không đúng OTP hoặc hết hạn!";
                        }
                        else
                        {
                            responseJson.success = "N";
                            responseJson.message = "Có lỗi xảy ra";
                        }

                        //Make any other calls using HttpClient here.

                        //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                        client.Dispose();
                    }

                    
                }
                else
                {
                    responseJson.success = "N";
                    responseJson.message = "Chưa nhập số điện thoại";
                }
            }
            catch (Exception ex)
            {

                responseJson.success = "N";
                responseJson.message = ex.Message;
            }
            return Json(responseJson);
        }

        [Route("nop-ho-so")]
        public IActionResult LoginToSubmit()
        {
            return View("SubmitFIle");
        }

        //public IActionResult VerifyOTP(LoginToSubmit loginToSubmit)
        //{
        //    string phoneNumberSession = HttpContext.Session.GetString("PhoneNumber");

        //    if (string.IsNullOrEmpty(phoneNumberSession))
        //    {
        //        if (!String.IsNullOrEmpty(loginToSubmit.PhoneNumber) && !String.IsNullOrEmpty(loginToSubmit.OTP))
        //        {
        //            //Lan dau check xem link co hop le khong
        //            string urlParameters = "?phoneNumber=" + loginToSubmit.PhoneNumber.Trim() + "&otp=" + loginToSubmit.OTP.Trim();
        //            HttpClient client = new HttpClient();
        //            client.BaseAddress = new Uri(Constant.URL_VERIFY_OTP);
        //            // Add an Accept header for JSON format.
        //            client.DefaultRequestHeaders.Accept.Add(
        //            new MediaTypeWithQualityHeaderValue("application/json"));

        //            // List data response.
        //            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
        //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //            {
        //                HttpContext.Session.SetString("PhoneNumber", SysFrameworks.Extensions.ToNormalPhoneNumber(loginToSubmit.PhoneNumber));
        //            }
        //            else
        //            {
        //                return Redirect("/nhapotp");
        //            }
        //        }
        //        else
        //        {
        //            return Redirect("/nhapotp");
        //        }
        //    }
        //}

        [Route("nhaphoso")]
        public IActionResult Getnfo()
        {
            string phoneNumberSession = HttpContext.Session.GetString("PhoneNumber");
            if (string.IsNullOrEmpty(phoneNumberSession))
            {
                return Redirect("/nhapotp");
            }
            else
            {
                try
                {
                    BCustomerWin bCustomerWin = _submitCustomerWinContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(phoneNumberSession))).FirstOrDefault<BCustomerWin>();
                    InfoCustomerWinDTO infoCustomerWinDTO = new InfoCustomerWinDTO();
                    infoCustomerWinDTO.ModelName = bCustomerWin.ModelName;
                    infoCustomerWinDTO.EngineNo = bCustomerWin.EngineNo;
                    infoCustomerWinDTO.FullNameInfo = bCustomerWin.CustomerName;
                    infoCustomerWinDTO.EngineNo = bCustomerWin.EngineNo;
                    infoCustomerWinDTO.PhoneNumber = bCustomerWin.PhoneNumber;
                    infoCustomerWinDTO.AddressInfo = bCustomerWin.Address;
                    infoCustomerWinDTO.PrizeWin = (bCustomerWin.PrizeWin == 1) ? "Giải Iphone 13" : "Giải hoàn tiền 5 triệu";
                    infoCustomerWinDTO.LuckyCode = bCustomerWin.LuckyCode;
                    ViewBag.TypeProduct = bCustomerWin.TypeProduct;
                    ViewBag.TypeWin = bCustomerWin.PrizeWin;

                    int countSubmitCustomer = _submitCustomerWinContext.submitInfoCustomerWins.
                        Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(phoneNumberSession))).Count();

                    SubmitInfoCustomerWin submitInfoCustomerWin = _submitCustomerWinContext.submitInfoCustomerWins.
                        Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(phoneNumberSession))).OrderByDescending(o => o.Id).FirstOrDefault<SubmitInfoCustomerWin>();

                    if (submitInfoCustomerWin != null)
                    {
                        infoCustomerWinDTO.Address = submitInfoCustomerWin.Address;
                        infoCustomerWinDTO.PhoneNumber = submitInfoCustomerWin.PhoneNumber;
                        infoCustomerWinDTO.BankAccountName = submitInfoCustomerWin.FullName;
                        infoCustomerWinDTO.BankAccountNameAuth = submitInfoCustomerWin.BankAccountName;
                        infoCustomerWinDTO.BankNameId = submitInfoCustomerWin.BankNameId;
                        infoCustomerWinDTO.BankNameIdAuth = submitInfoCustomerWin.BankNameId;
                        infoCustomerWinDTO.DistrictsId = submitInfoCustomerWin.DistrictsId;
                        infoCustomerWinDTO.ProvinceId = submitInfoCustomerWin.ProvinceId;
                        infoCustomerWinDTO.WardId = submitInfoCustomerWin.WardId;
                        infoCustomerWinDTO.BankNumber = submitInfoCustomerWin.BankNumber;
                        infoCustomerWinDTO.BankNumberAuth = submitInfoCustomerWin.BankNumber;
                        infoCustomerWinDTO.FullName = submitInfoCustomerWin.FullName;
                        infoCustomerWinDTO.BankBranch = submitInfoCustomerWin.BankBranch;
                        infoCustomerWinDTO.BankBranchAuth = submitInfoCustomerWin.BankBranch;
                        infoCustomerWinDTO.ProvinceBranchId = submitInfoCustomerWin.ProvinceBranchId;
                        infoCustomerWinDTO.ProvinceBranchIdAuth = submitInfoCustomerWin.ProvinceBranchId;
                        infoCustomerWinDTO.IdentificationNumber = submitInfoCustomerWin.IdentificationNumber;
                        infoCustomerWinDTO.IdentificationAuthorization = submitInfoCustomerWin.IdentificationAuthorization;
                        infoCustomerWinDTO.TypeBank = String.IsNullOrEmpty(infoCustomerWinDTO.IdentificationAuthorization) ? 1 : 2;
                        infoCustomerWinDTO.Status = submitInfoCustomerWin.CustomerStatus;
                        ViewBag.TypeWin = bCustomerWin.PrizeWin;
                        ViewBag.ReasonRefusal = submitInfoCustomerWin.NOTE_CUS_REJECT;
                        MStatus mStatus = _masterContext.mStatuses.Find(submitInfoCustomerWin.CustomerStatus);
                        if (mStatus != null)
                        {
                            ViewBag.Status = mStatus.Id;
                            ViewBag.StatusMess = mStatus.Status;
                        }

                        //if (submitInfoCustomerWin.CustomerStatus == Constant.C_STATUS_CUS_TU_CHOI && submitInfoCustomerWin.REJECT_DATE != null && DateTime.Now.AddDays(-3) < submitInfoCustomerWin.REJECT_DATE)//bị từ chối
                        if (submitInfoCustomerWin.CustomerStatus == Constant.C_STATUS_CUS_TU_CHOI && bCustomerWin.RoundId == Constant.ROUND_ID_ACTIVE)
                        //if (submitInfoCustomerWin.CustomerStatus == Constant.C_STATUS_CUS_TU_CHOI)
                        {
                            ViewBag.IsUpdate = "Y";
                        }
                        else
                        {
                            ViewBag.IsUpdate = "N";
                        }
                    }
                    else
                    {
                        if (bCustomerWin.RoundId == Constant.ROUND_ID_ACTIVE)
                        {
                            ViewBag.IsUpdate = "Y";
                        }
                        else
                        {
                            ViewBag.IsUpdate = "N";
                        }
                        //ViewBag.IsUpdate = "Y";
                    }
                    List<SelectListItem> provinces = _appRegisterContext.M_PROVINCE
                            .Select(a => new SelectListItem()
                            {
                                Value = a.ID.ToString(),
                                Text = a.PROVINCE_NAME
                            }).ToList();
                    List<SelectListItem> districts = _appRegisterContext.M_DISTRICT.Where(p => p.PROVINCE_ID == (submitInfoCustomerWin == null ? 0 : submitInfoCustomerWin.ProvinceId))
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.DISTRICT_NAME
                    }).ToList();
                    List<SelectListItem> banks = _masterContext.mBanks
                    .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.BankName
                    }).ToList();
                    List<SelectListItem> mWards = _masterContext.mWards.Where(p => p.DistrictId == (submitInfoCustomerWin == null ? 0 : submitInfoCustomerWin.DistrictsId))
                    .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.WardName
                    }).ToList();
                    provinces.Insert(0, new SelectListItem("", ""));
                    districts.Insert(0, new SelectListItem("", ""));
                    banks.Insert(0, new SelectListItem("", ""));
                    mWards.Insert(0, new SelectListItem("", ""));
                    ViewBag.listProvince = provinces;
                    ViewBag.listDistrict = districts;
                    ViewBag.listBank = banks;
                    ViewBag.listWard = mWards;
                    ViewData["infoCustomerWin"] = infoCustomerWinDTO;
                    return View("SubmitInfo", infoCustomerWinDTO);
                }
                catch (Exception ex)
                {
                    //redirect toi view loi
                    return View("SubmitInfo");
                }
            }


        }

        [Route("nopthongtin")]
        public IActionResult SubmitInfo(InfoCustomerWinDTO infoCustomerWinDTO)
        {
            try
            {
                //check trang thai trong truong hop go link truc tiep
                string phoneNumberSession = HttpContext.Session.GetString("PhoneNumber");
                if (string.IsNullOrEmpty(phoneNumberSession))
                {
                    phoneNumberSession = SysFrameworks.Extensions.ToNormalPhoneNumber(infoCustomerWinDTO.PhoneNumber);
                }
                if (!string.IsNullOrEmpty(phoneNumberSession))
                {

                    List<SubmitInfoCustomerWin> submitInfoCustomerWins = _submitCustomerWinContext.submitInfoCustomerWins.
                        Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(infoCustomerWinDTO.PhoneNumber))).ToList<SubmitInfoCustomerWin>();
                    SubmitInfoCustomerWin submitInfoCustomerWinCheck = _submitCustomerWinContext.submitInfoCustomerWins.
                     Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(phoneNumberSession))).OrderByDescending(o => o.Id).FirstOrDefault<SubmitInfoCustomerWin>();
                    //if (submitInfoCustomerWins != null && submitInfoCustomerWins.Count >= 2)
                    //{
                    //    return Redirect("/nhaphoso");
                    //}
                    //else
                    //{
                    if (
                        submitInfoCustomerWins == null
                        || (submitInfoCustomerWins != null && submitInfoCustomerWins.Count == 0)
                        || (submitInfoCustomerWins != null && submitInfoCustomerWins.Count >= 1 && submitInfoCustomerWinCheck.CustomerStatus == SysFrameworks.Constant.C_STATUS_CUS_TU_CHOI)
                        )
                    {
                        BCustomerWin bCustomerWin = _submitCustomerWinContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(infoCustomerWinDTO.PhoneNumber))).FirstOrDefault<BCustomerWin>();
                        SubmitInfoCustomerWin submitInfoCustomerWin = new SubmitInfoCustomerWin();

                        submitInfoCustomerWin.IdentificationNumber = infoCustomerWinDTO.IdentificationNumber;
                        submitInfoCustomerWin.PhoneNumber = bCustomerWin.PhoneNumber;
                        submitInfoCustomerWin.ModelName = bCustomerWin.ModelName;
                        submitInfoCustomerWin.FullName = infoCustomerWinDTO.FullName;
                        submitInfoCustomerWin.Address = infoCustomerWinDTO.Address;
                        submitInfoCustomerWin.EngineNo = bCustomerWin.EngineNo;
                        submitInfoCustomerWin.ProvinceId = infoCustomerWinDTO.ProvinceId;
                        submitInfoCustomerWin.DistrictsId = infoCustomerWinDTO.DistrictsId;
                        submitInfoCustomerWin.WardId = infoCustomerWinDTO.WardId;
                        submitInfoCustomerWin.BankNumber = infoCustomerWinDTO.BankNumber;
                        submitInfoCustomerWin.BankNameId = infoCustomerWinDTO.BankNameId;
                        submitInfoCustomerWin.BankBranch = infoCustomerWinDTO.BankBranch;
                        submitInfoCustomerWin.IdentificationAuthorization = infoCustomerWinDTO.IdentificationAuthorization;
                        submitInfoCustomerWin.BankAccountName = infoCustomerWinDTO.BankAccountName;
                        submitInfoCustomerWin.NOTE_OF_CUSTOMER = infoCustomerWinDTO.NoteOfCus;
                        submitInfoCustomerWin.CreateDate = DateTime.Now;
                        submitInfoCustomerWin.CustomerStatus = 8; //khách hàng submit
                        submitInfoCustomerWin.PanStatus = 8;//khách hàng submit
                        if (submitInfoCustomerWins.Count > 0)
                        {
                            submitInfoCustomerWin.CountSubmit = submitInfoCustomerWins.Count + 1;
                        }
                        else
                        {
                            submitInfoCustomerWin.CountSubmit = 1;
                        }

                        if (infoCustomerWinDTO.TypeBank == 2)
                        {
                            submitInfoCustomerWin.BankNumber = infoCustomerWinDTO.BankNumberAuth;
                            submitInfoCustomerWin.BankNameId = infoCustomerWinDTO.BankNameIdAuth;
                            submitInfoCustomerWin.BankBranch = infoCustomerWinDTO.BankBranchAuth;
                            submitInfoCustomerWin.IdentificationAuthorization = infoCustomerWinDTO.IdentificationAuthorization;
                            submitInfoCustomerWin.BankAccountName = String.IsNullOrEmpty(infoCustomerWinDTO.BankAccountNameAuth) ? infoCustomerWinDTO.BankAccountNameAuth : CString.RemoveUnicode(infoCustomerWinDTO.BankAccountNameAuth).ToUpper();
                            submitInfoCustomerWin.ProvinceBranchId = infoCustomerWinDTO.ProvinceBranchIdAuth;
                        }
                        else
                        {
                            submitInfoCustomerWin.BankNumber = infoCustomerWinDTO.BankNumber;
                            submitInfoCustomerWin.BankNameId = infoCustomerWinDTO.BankNameId;
                            submitInfoCustomerWin.BankBranch = infoCustomerWinDTO.BankBranch;
                            submitInfoCustomerWin.BankAccountName = String.IsNullOrEmpty(infoCustomerWinDTO.BankAccountName) ? infoCustomerWinDTO.BankAccountName : CString.RemoveUnicode(infoCustomerWinDTO.BankAccountName).ToUpper();
                            submitInfoCustomerWin.ProvinceBranchId = infoCustomerWinDTO.ProvinceBranchId;
                        }

                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string foldersName = Path.Combine(wwwRootPath, "DocumentsSubmit");

                        var formFileFront = infoCustomerWinDTO.FileIDFront;
                        var formFileBackside = infoCustomerWinDTO.FileIDBackside;
                        var formFileSerialNumber = infoCustomerWinDTO.FileSerialNumber;
                        var formFileSMSCode = infoCustomerWinDTO.FileSMSCode;
                        var formFileSMSWin = infoCustomerWinDTO.FileSMSWin;
                        var formFileAuthozizationLetter = infoCustomerWinDTO.FileAuthozizationLetter;
                        var formFileInvoice = infoCustomerWinDTO.FileInvoice;
                        var formFileProduct = infoCustomerWinDTO.FileProduct;

                        var formFileFrontAuthor = infoCustomerWinDTO.FILE_ID_FRONT_AUTHORIZATION;
                        var formFileBacksideAuthor = infoCustomerWinDTO.FILE_ID_BACKSIDE_AUTHORIZATION;
                        if (formFileFront != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileFront.FileName);
                            string extension = Path.GetExtension(formFileFront.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileFront.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FileIDFront = fileNameServer;
                        }

                        if (formFileBackside != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileBackside.FileName);
                            string extension = Path.GetExtension(formFileBackside.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileBackside.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FileIDBackside = fileNameServer;
                        }

                        if (formFileFrontAuthor != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileFrontAuthor.FileName);
                            string extension = Path.GetExtension(formFileFrontAuthor.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileFrontAuthor.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FILE_ID_FRONT_AUTHORIZATION = fileNameServer;
                        }

                        if (formFileBacksideAuthor != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileBacksideAuthor.FileName);
                            string extension = Path.GetExtension(formFileBacksideAuthor.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileBacksideAuthor.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FILE_ID_BACKSIDE_AUTHORIZATION = fileNameServer;
                        }

                        if (formFileAuthozizationLetter != null)
                        {
                            string fileNameFileAuthozizationLetter = "";
                            foreach (var formFile in formFileAuthozizationLetter)
                            {
                                string originalFileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                                string extension = Path.GetExtension(formFile.FileName);
                                string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                                var filePath = Path.Combine(foldersName, fileNameServer);
                                using (var filestream = new FileStream(filePath, FileMode.Create))
                                {
                                    formFile.CopyTo(filestream);
                                }
                                fileNameFileAuthozizationLetter += (fileNameFileAuthozizationLetter == "") ? fileNameServer : (";" + fileNameServer);
                            }
                            submitInfoCustomerWin.FileAuthozizationLetter = fileNameFileAuthozizationLetter;
                        }

                        if (formFileSerialNumber != null && formFileSerialNumber.Count > 0)
                        {
                            string fileNameFileSerialNumber = "";
                            foreach (var formFile in formFileSerialNumber)
                            {
                                if (formFile.Length > 0)
                                {
                                    string originalFileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                                    string extension = Path.GetExtension(formFile.FileName);
                                    string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                                    var filePath = Path.Combine(foldersName, fileNameServer);
                                    using (var filestream = new FileStream(filePath, FileMode.Create))
                                    {
                                        formFile.CopyTo(filestream);
                                    }
                                    fileNameFileSerialNumber += (fileNameFileSerialNumber == "") ? fileNameServer : (";" + fileNameServer);
                                }
                            }
                            submitInfoCustomerWin.FileSerialNumber = fileNameFileSerialNumber;
                        }

                        if (formFileSMSCode != null && formFileSMSCode.Count > 0)
                        {
                            string fileNameFileSMSCode = "";
                            foreach (var formFile in formFileSMSCode)
                            {
                                if (formFile.Length > 0)
                                {
                                    string originalFileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                                    string extension = Path.GetExtension(formFile.FileName);
                                    string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                                    var filePath = Path.Combine(foldersName, fileNameServer);
                                    using (var filestream = new FileStream(filePath, FileMode.Create))
                                    {
                                        formFile.CopyTo(filestream);
                                    }
                                    fileNameFileSMSCode += (fileNameFileSMSCode == "") ? fileNameServer : (";" + fileNameServer);
                                }
                            }
                            submitInfoCustomerWin.FileSMSCode = fileNameFileSMSCode;
                        }

                        if (formFileSMSWin != null)
                        {
                            string fileNameFileSMSWin = "";
                            foreach (var formFile in formFileSMSWin)
                            {
                                if (formFile.Length > 0)
                                {
                                    string originalFileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                                    string extension = Path.GetExtension(formFile.FileName);
                                    string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                                    var filePath = Path.Combine(foldersName, fileNameServer);
                                    using (var filestream = new FileStream(filePath, FileMode.Create))
                                    {
                                        formFile.CopyTo(filestream);
                                    }
                                    fileNameFileSMSWin += (fileNameFileSMSWin == "") ? fileNameServer : (";" + fileNameServer);
                                }
                            }
                            submitInfoCustomerWin.FileSMSWin = fileNameFileSMSWin;
                        }

                        if (formFileProduct != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileProduct.FileName);
                            string extension = Path.GetExtension(formFileProduct.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileProduct.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FileProduct = fileNameServer;
                        }

                        if (formFileInvoice != null)
                        {
                            string originalFileName = Path.GetFileNameWithoutExtension(formFileInvoice.FileName);
                            string extension = Path.GetExtension(formFileInvoice.FileName);
                            string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                            var filePath = Path.Combine(foldersName, fileNameServer);
                            using (var filestream = new FileStream(filePath, FileMode.Create))
                            {
                                formFileInvoice.CopyTo(filestream);
                            }
                            submitInfoCustomerWin.FileInvoice = fileNameServer;
                        }
                        _submitCustomerWinContext.submitInfoCustomerWins.Add(submitInfoCustomerWin);
                        _submitCustomerWinContext.SaveChanges();
                        HttpContext.Session.Remove("PhoneNumber");
                        return Redirect("/guihosothanhcong");
                    }
                    else
                    {
                        return Redirect("/nhaphoso");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Phiên xử lý đã hết hạn. Hãy thực hiện lại!";
                    return View("SubmitInfo");
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("SubmitInfo");
            }

        }

        [Route("suathongtinhoso")]
        public IActionResult GetInfoFileById(long id)
        {
            InfoCustomerWinDTO infoCustomerWinDTO = new InfoCustomerWinDTO();
            try
            {
                //string phoneNumber = HttpContext.Session.GetString("PhoneNumber");
                SubmitInfoCustomerWin submitInfoCustomerWin = _submitCustomerWinContext.submitInfoCustomerWins.Find(id);
                BCustomerWin bCustomerWin = _submitCustomerWinContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(submitInfoCustomerWin.PhoneNumber)).FirstOrDefault<BCustomerWin>();
                if (bCustomerWin != null)
                {
                    infoCustomerWinDTO.ModelName = bCustomerWin.ModelName;
                    infoCustomerWinDTO.EngineNo = bCustomerWin.EngineNo;
                    if (submitInfoCustomerWin != null)
                    {
                        infoCustomerWinDTO.Address = submitInfoCustomerWin.Address;
                        infoCustomerWinDTO.PhoneNumber = submitInfoCustomerWin.PhoneNumber;
                        infoCustomerWinDTO.BankAccountName = submitInfoCustomerWin.BankAccountName;
                        infoCustomerWinDTO.BankNameId = submitInfoCustomerWin.BankNameId;
                        infoCustomerWinDTO.DistrictsId = submitInfoCustomerWin.DistrictsId;
                        infoCustomerWinDTO.ProvinceId = submitInfoCustomerWin.ProvinceId;
                        infoCustomerWinDTO.BankNumber = submitInfoCustomerWin.BankNumber;
                        infoCustomerWinDTO.FullName = submitInfoCustomerWin.FullName;
                        infoCustomerWinDTO.IdentificationNumber = submitInfoCustomerWin.IdentificationNumber;
                        infoCustomerWinDTO.Status = submitInfoCustomerWin.CustomerStatus;
                        ViewBag.TypeWin = bCustomerWin.PrizeWin;
                        MStatus mStatus = _masterContext.mStatuses.Find(submitInfoCustomerWin.CustomerStatus);
                        if (mStatus != null)
                        {
                            ViewBag.Status = mStatus.Status;
                        }

                        ViewBag.pathFrontPath = "/Documents/" + submitInfoCustomerWin.FileIDFront;
                        ViewBag.pathBacksidePath = "/Documents/" + submitInfoCustomerWin.FileIDBackside;
                        ViewBag.pathFileProduct = "/Documents/" + submitInfoCustomerWin.FileProduct;
                        ViewBag.FileInvoice = "/Documents/" + submitInfoCustomerWin.FileInvoice;
                        //list anh serial
                        List<string> listImagesSerial = new List<string>();
                        foreach (string image in submitInfoCustomerWin.FileSerialNumber.Split(";"))
                        {
                            listImagesSerial.Add("/Documents/" + image);
                        }
                        ViewData["image-serial"] = listImagesSerial;

                        List<string> listImagesSMS = new List<string>();
                        foreach (string image in submitInfoCustomerWin.FileSMSCode.Split(";"))
                        {
                            listImagesSMS.Add("/Documents/" + image);
                        }
                        ViewData["image-sms"] = listImagesSMS;
                    }

                    List<SelectListItem> provinces = _appRegisterContext.M_PROVINCE
                        .Select(a => new SelectListItem()
                        {
                            Value = a.ID.ToString(),
                            Text = a.PROVINCE_NAME
                        }).ToList();
                    List<SelectListItem> districts = _appRegisterContext.M_DISTRICT
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.DISTRICT_NAME
                    }).ToList();
                    provinces.Insert(0, new SelectListItem("", ""));
                    districts.Insert(0, new SelectListItem("", ""));
                    ViewBag.listProvince = provinces;
                    ViewBag.listDistrict = districts;
                }
            }
            catch (Exception ex)
            {
                ViewBag.errMessage = ex.Message;

            }
            return View("/EditFileInfo", infoCustomerWinDTO);
        }


    }
}
