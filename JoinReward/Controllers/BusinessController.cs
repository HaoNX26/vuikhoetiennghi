﻿using ICSharpCode.SharpZipLib.Zip;
using JoinReward.Models;
using JoinReward.Models.DB;
using JoinReward.Models.DTO;
using JoinReward.Models.JoinReward;
using JoinReward.Models.Master;
using JoinReward.Models.SubmitCustomerWin;
using JoinReward.Models.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAN.Warranty.Models.Master.JoinReward;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JoinReward.Controllers
{
    public class BusinessController : BaseController
    {
        private readonly Models.DB.MasterContext _masterContext;
        private readonly Models.DB.BusinessContext _businessContext;
        private readonly Models.DB.UserContext _userContext;
        private readonly AppRegisterContext _appRegisterContext;
        private readonly IHostingEnvironment _hostEnvironment;
        public BusinessController(Models.DB.MasterContext masterContext, Models.DB.BusinessContext businessContext, Models.DB.UserContext userContext, IHostingEnvironment hostEnvironment, AppRegisterContext appRegisterContext)
        {
            _masterContext = masterContext;
            _businessContext = businessContext;
            _userContext = userContext;
            _hostEnvironment = hostEnvironment;
            _appRegisterContext = appRegisterContext;
        }
        private string GetFunctionName(long fid)
        {
            if (fid.ToString() == SysFrameworks.Constant.C_ROLE_ASC)
            {
                return "Xử lý hồ sơ";
            }
            else if (fid.ToString() == SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
            {
                return "Giao việc cho ASC";
            }
            else if (fid.ToString() == SysFrameworks.Constant.C_ROLE_CALL_CENTER)
            {
                return "Xử lý hồ sơ";
            } else if (fid.ToString().Equals(SysFrameworks.Constant.C_F_APPROVE))
            {
                return "Duyệt hồ sơ";
            }
            else if (fid.ToString().Equals(SysFrameworks.Constant.C_F_REJECT))
            {
                return "Từ chối hồ sơ";
            }
            else if (fid.ToString().Equals(SysFrameworks.Constant.C_F_REWARD))
            {
                return "Trả thưởng";
            }
            return "";
        }
        public IActionResult ViewFileCustomerWin(FileCustomerWinDTO fileCustomerWinDTO)
        {
            ViewData["Title"] = GetFunctionName(int.Parse(fileCustomerWinDTO.fid));

            HttpContext.Session.SetString("fid", fileCustomerWinDTO.fid);
            try
            {
                S_USER s_USER = _masterContext.S_USER.Find((long)HttpContext.Session.GetInt32("s_user_id").Value);
                ASCMaster aSCMaster = _masterContext.aSCMasters.Where(p => p.ID == s_USER.ASC_ID).FirstOrDefault<ASCMaster>();

                //chức năng duyệt, từ chối, trả thưởng
                if (fileCustomerWinDTO.fid.ToString().Equals(SysFrameworks.Constant.C_F_APPROVE))
                {
                    fileCustomerWinDTO.PAN_STATUS = SysFrameworks.Constant.C_STATUS_HO_SO_DU;
                }
                else if (fileCustomerWinDTO.fid.ToString().Equals(SysFrameworks.Constant.C_F_REJECT))
                {
                    fileCustomerWinDTO.PAN_STATUS = SysFrameworks.Constant.C_STATUS_HO_SO_THIEU;
                }
                else if (fileCustomerWinDTO.fid.ToString().Equals(SysFrameworks.Constant.C_F_REWARD))
                {
                    fileCustomerWinDTO.CUSTOMER_STATUS = SysFrameworks.Constant.C_STATUS_CUS_DUYET;
                }


                if (aSCMaster.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC)) // acc login là ASC
                {
                    fileCustomerWinDTO.AscId = aSCMaster.ID; //Fix cứng đk search
                    //fileCustomerWinDTO.StatusId = SysFrameworks.Constant.C_STATUS_CUS_DA_NOP;
                }
                ViewBag.Roles = s_USER.ROLES;
                ViewBag.Fid = fileCustomerWinDTO.fid;
                if (s_USER.ROLES.Equals(SysFrameworks.Constant.C_ROLE_AGENCY))
                {
                    fileCustomerWinDTO.AGENCY_ID = s_USER.ID;
                }
                List<SelectListItem> ASCList = _masterContext.aSCMasters.Where(p => p.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC))
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.ASC_CODE
                    }).ToList();
                ASCList.Insert(0, new SelectListItem("", ""));
                if (fileCustomerWinDTO.PAGE_SIZE <= 0) fileCustomerWinDTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                if (fileCustomerWinDTO.page <= 0) fileCustomerWinDTO.page = 1;


                //Status ASC
                const long C_TYPE_ASC_STATUS = 3;
                List<SelectListItem> lsASC_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_ASC_STATUS)
                        .Select(a => new SelectListItem()
                        {
                            Value = a.Id.ToString(),
                            Text = a.Status
                        }).ToList();
                lsASC_STATUS.Insert(0, new SelectListItem("", ""));
                ViewBag.lsASC_STATUS = lsASC_STATUS;

                //Status PAN
                const long C_TYPE_PAN_STATUS = 1;
                const long C_ID_PAN_STATUS_DA_NOP_HS = 8;
                List<SelectListItem> lsPAN_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_PAN_STATUS || p.Id == C_ID_PAN_STATUS_DA_NOP_HS)
                        .Select(a => new SelectListItem()
                        {
                            Value = a.Id.ToString(),
                            Text = a.Status
                        }).ToList();
                lsPAN_STATUS.Insert(0, new SelectListItem("", ""));
                ViewBag.lsPAN_STATUS = lsPAN_STATUS;


                //Cus status
                const long C_TYPE_CUSTOMER_STATUS = 2;
                List<SelectListItem> lsCUS_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_CUSTOMER_STATUS)
                        .Select(a => new SelectListItem()
                        {
                            Value = a.Id.ToString(),
                            Text = a.Status
                        }).ToList();
                lsCUS_STATUS.Insert(0, new SelectListItem("", ""));
                ViewBag.lsCUS_STATUS = lsCUS_STATUS;

                List<SelectListItem> lsPronvice = _appRegisterContext.M_PROVINCE.ToList()
                       .Select(a => new SelectListItem()
                       {
                           Value = a.ID.ToString(),
                           Text = a.PROVINCE_NAME
                       }).ToList();
                lsPronvice.Insert(0, new SelectListItem("", ""));
                ViewBag.lsPronvice = lsPronvice;

                List<SelectListItem> lsRound = new List<SelectListItem>();
                lsRound.Insert(0, new SelectListItem("", ""));
                lsRound.Insert(1, new SelectListItem("Vòng 1", "1"));
                lsRound.Insert(2, new SelectListItem("Vòng 2", "2"));
                lsRound.Insert(3, new SelectListItem("Vòng 3", "3"));
                lsRound.Insert(4, new SelectListItem("Vòng 4", "4"));
                ViewBag.lsRound = lsRound;

                List<SelectListItem> ListAgency = _masterContext.S_USER.Where(p => p.ROLES.Equals(SysFrameworks.Constant.C_ROLE_AGENCY))
                   .Select(a => new SelectListItem()
                   {
                       Value = a.ID.ToString(),
                       Text = a.FULLNAME
                   }).ToList();
                ListAgency.Insert(0, new SelectListItem("", ""));
                ViewBag.ListAgency = ListAgency;

                List<SelectListItem> lsPrizeWin = new List<SelectListItem>();
                lsPrizeWin.Insert(0, new SelectListItem("", ""));
                lsPrizeWin.Insert(1, new SelectListItem("IPhone", "1"));
                lsPrizeWin.Insert(2, new SelectListItem("Hoàn tiền", "2"));
                ViewBag.lsPrizeWin = lsPrizeWin;

                List<SelectListItem> lsCountSubmit = new List<SelectListItem>();
                lsCountSubmit.Insert(0, new SelectListItem("", ""));
                lsCountSubmit.Insert(1, new SelectListItem("Lần 1", "1"));
                lsCountSubmit.Insert(2, new SelectListItem("Lần 2", "2"));
                lsCountSubmit.Insert(3, new SelectListItem("Lần 3", "3"));
                lsCountSubmit.Insert(4, new SelectListItem("Lần 4", "4"));
                lsCountSubmit.Insert(5, new SelectListItem("Lần 5", "5"));
                ViewBag.lsCountSubmit = lsCountSubmit;

                Models.Pagination pagination = new Models.Pagination()
                {
                    PAGE_SIZE = fileCustomerWinDTO.PAGE_SIZE,
                    CUR_PAGE = fileCustomerWinDTO.page
                };
                List<FileCustomerWinSearchDTO> fileCustomerWinDtos = _businessContext.SearchFileCustomerWin(fileCustomerWinDTO, pagination.CUR_PAGE, pagination.PAGE_SIZE);
                if (fileCustomerWinDtos.Count > 0) pagination.TOTAL_RECORD = fileCustomerWinDtos[0].NUM_OF_RECORD;
                pagination.CONTROLLER = ControllerContext.ActionDescriptor.ControllerName;
                pagination.ACTION = ControllerContext.ActionDescriptor.ActionName;
                //L?y thu?c tính truy?n qua query string
                DataCollections cols = new DataCollections();
                cols.Add(DataTypes.NVarchar, "CustmerName", FieldTypes.DefaultValue, fileCustomerWinDTO.CustmerName, "");
                cols.Add(DataTypes.NVarchar, "LuckyCode", FieldTypes.DefaultValue, fileCustomerWinDTO.LuckyCode, "");
                cols.Add(DataTypes.NVarchar, "PhoneNumber", FieldTypes.DefaultValue, fileCustomerWinDTO.PhoneNumber, "");
                cols.Add(DataTypes.NVarchar, "AscId", FieldTypes.DefaultValue, fileCustomerWinDTO.AscId, "");
                cols.Add(DataTypes.NVarchar, "PRIZE_WIN", FieldTypes.DefaultValue, fileCustomerWinDTO.PRIZE_WIN, "");
                cols.Add(DataTypes.NVarchar, "ROUND_ID", FieldTypes.DefaultValue, fileCustomerWinDTO.ROUND_ID, "");
                cols.Add(DataTypes.NVarchar, "CUSTOMER_STATUS", FieldTypes.DefaultValue, fileCustomerWinDTO.CUSTOMER_STATUS, "");
                cols.Add(DataTypes.NVarchar, "PAN_STATUS", FieldTypes.DefaultValue, fileCustomerWinDTO.PAN_STATUS, "");
                cols.Add(DataTypes.NVarchar, "ASC_STATUS", FieldTypes.DefaultValue, fileCustomerWinDTO.ASC_STATUS, "");
                cols.Add(DataTypes.NVarchar, "PROVINCE_ID", FieldTypes.DefaultValue, fileCustomerWinDTO.PROVINCE_ID, "");
                cols.Add(DataTypes.NVarchar, "COUNT_SUBMIT", FieldTypes.DefaultValue, fileCustomerWinDTO.COUNT_SUBMIT, "");
                cols.Add(DataTypes.NVarchar, "AGENCY_ID", FieldTypes.DefaultValue, fileCustomerWinDTO.AGENCY_ID, "");
                cols.Add(DataTypes.NVarchar, "FROM_DATE", FieldTypes.DefaultValue, fileCustomerWinDTO.FROM_DATE, "");
                cols.Add(DataTypes.NVarchar, "TO_DATE", FieldTypes.DefaultValue, fileCustomerWinDTO.TO_DATE, "");
                cols.Add(DataTypes.NVarchar, "fid", FieldTypes.DefaultValue, fileCustomerWinDTO.fid, "");
                pagination.GET_QUERY_STRING = SysFrameworks.Url.to_Get_Param(cols);
                ViewData["pagination"] = pagination;
                ViewData["grid_data"] = fileCustomerWinDtos;
                ViewBag.ListASC = ASCList;
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message.ToString();
            }
            return View("ViewFileCustomerWin", fileCustomerWinDTO);
        }

        public IActionResult ViewListWinner(FileCustomerWinDTO fileCustomerWinDTO)
        {
            ViewData["Title"] = GetFunctionName(int.Parse(fileCustomerWinDTO.fid));

            HttpContext.Session.SetString("fid", fileCustomerWinDTO.fid);
            try
            {
                if (fileCustomerWinDTO.PAGE_SIZE <= 0) fileCustomerWinDTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                if (fileCustomerWinDTO.page <= 0) fileCustomerWinDTO.page = 1;

                S_USER s_USER = _masterContext.S_USER.Find((long)HttpContext.Session.GetInt32("s_user_id").Value);
                ASCMaster aSCMaster = _masterContext.aSCMasters.Where(p => p.ID == s_USER.ASC_ID).FirstOrDefault<ASCMaster>();

                if (aSCMaster.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC)) // acc login là ASC
                {
                    fileCustomerWinDTO.AscId = aSCMaster.ID; //Fix cứng đk search
                    //fileCustomerWinDTO.StatusId = SysFrameworks.Constant.C_STATUS_CUS_DA_NOP;
                }
                ViewBag.Roles = s_USER.ROLES;
                ViewBag.Fid = fileCustomerWinDTO.fid;
                if (s_USER.ROLES.Equals(SysFrameworks.Constant.C_ROLE_AGENCY))
                {
                    fileCustomerWinDTO.AGENCY_ID = s_USER.ID;
                }
                //List<SelectListItem> ASCList = _masterContext.aSCMasters.Where(p => p.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC))
                //    .Select(a => new SelectListItem()
                //    {
                //        Value = a.ID.ToString(),
                //        Text = a.ASC_CODE
                //    }).ToList();
                //ASCList.Insert(0, new SelectListItem("", ""));
                //if (fileCustomerWinDTO.PAGE_SIZE <= 0) fileCustomerWinDTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                //if (fileCustomerWinDTO.page <= 0) fileCustomerWinDTO.page = 1;


                ////Status ASC
                //const long C_TYPE_ASC_STATUS = 3;
                //List<SelectListItem> lsASC_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_ASC_STATUS)
                //        .Select(a => new SelectListItem()
                //        {
                //            Value = a.Id.ToString(),
                //            Text = a.Status
                //        }).ToList();
                //lsASC_STATUS.Insert(0, new SelectListItem("", ""));
                //ViewBag.lsASC_STATUS = lsASC_STATUS;

                ////Status PAN
                //const long C_TYPE_PAN_STATUS = 1;
                //const long C_ID_PAN_STATUS_DA_NOP_HS = 8;
                //List<SelectListItem> lsPAN_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_PAN_STATUS || p.Id == C_ID_PAN_STATUS_DA_NOP_HS)
                //        .Select(a => new SelectListItem()
                //        {
                //            Value = a.Id.ToString(),
                //            Text = a.Status
                //        }).ToList();
                //lsPAN_STATUS.Insert(0, new SelectListItem("", ""));
                //ViewBag.lsPAN_STATUS = lsPAN_STATUS;


                //Cus status
                //const long C_TYPE_CUSTOMER_STATUS = 2;
                //List<SelectListItem> lsCUS_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_CUSTOMER_STATUS)
                //        .Select(a => new SelectListItem()
                //        {
                //            Value = a.Id.ToString(),
                //            Text = a.Status
                //        }).ToList();
                //lsCUS_STATUS.Insert(0, new SelectListItem("", ""));
                //ViewBag.lsCUS_STATUS = lsCUS_STATUS;

                //List<SelectListItem> lsPronvice = _appRegisterContext.M_PROVINCE.ToList()
                //       .Select(a => new SelectListItem()
                //       {
                //           Value = a.ID.ToString(),
                //           Text = a.PROVINCE_NAME
                //       }).ToList();
                //lsPronvice.Insert(0, new SelectListItem("", ""));
                //ViewBag.lsPronvice = lsPronvice;

                List<SelectListItem> lsRound = new List<SelectListItem>();
                lsRound.Insert(0, new SelectListItem("", ""));
                lsRound.Insert(1, new SelectListItem("Vòng 1", "1"));
                lsRound.Insert(2, new SelectListItem("Vòng 2", "2"));
                lsRound.Insert(3, new SelectListItem("Vòng 3", "3"));
                lsRound.Insert(4, new SelectListItem("Vòng 4", "4"));
                ViewBag.lsRound = lsRound;

               
                List<SelectListItem> lsPrizeWin = new List<SelectListItem>();
                lsPrizeWin.Insert(0, new SelectListItem("", ""));
                lsPrizeWin.Insert(1, new SelectListItem("IPhone", "1"));
                lsPrizeWin.Insert(2, new SelectListItem("Hoàn tiền", "2"));
                ViewBag.lsPrizeWin = lsPrizeWin;

 

                Models.Pagination pagination = new Models.Pagination()
                {
                    PAGE_SIZE = fileCustomerWinDTO.PAGE_SIZE,
                    CUR_PAGE = fileCustomerWinDTO.page
                };
                List<ListWinnerSearchDTO> listWinnerSearchDTOs = _businessContext.SearchWinner(fileCustomerWinDTO, pagination.CUR_PAGE, pagination.PAGE_SIZE);
                if (listWinnerSearchDTOs.Count > 0) pagination.TOTAL_RECORD = listWinnerSearchDTOs[0].NUM_OF_RECORD;
                pagination.CONTROLLER = ControllerContext.ActionDescriptor.ControllerName;
                pagination.ACTION = ControllerContext.ActionDescriptor.ActionName;
                //L?y thu?c tính truy?n qua query string
                DataCollections cols = new DataCollections();
                cols.Add(DataTypes.NVarchar, "CustmerName", FieldTypes.DefaultValue, fileCustomerWinDTO.CustmerName, "");
                cols.Add(DataTypes.NVarchar, "LuckyCode", FieldTypes.DefaultValue, fileCustomerWinDTO.LuckyCode, "");
                cols.Add(DataTypes.NVarchar, "PhoneNumber", FieldTypes.DefaultValue, fileCustomerWinDTO.PhoneNumber, "");
                cols.Add(DataTypes.NVarchar, "COUNT_SUBMIT", FieldTypes.DefaultValue, fileCustomerWinDTO.COUNT_SUBMIT, "");
                cols.Add(DataTypes.NVarchar, "fid", FieldTypes.DefaultValue, fileCustomerWinDTO.fid, "");
                pagination.GET_QUERY_STRING = SysFrameworks.Url.to_Get_Param(cols);
                ViewData["pagination"] = pagination;
                ViewData["grid_data"] = listWinnerSearchDTOs;
                //ViewBag.ListASC = ASCList;
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message.ToString();
            }
            return View("ViewListWinner", fileCustomerWinDTO);
        }

            public JsonResult updateStatusFileCustomerWin(string listId)
        {
            int fid = int.Parse(HttpContext.Session.GetString("fid"));
            JsonResponse response = new JsonResponse();
            SubmitInfoCustomerWin submitInfoCustomerWin = new SubmitInfoCustomerWin();
            
                try
                {
                    if (listId != null)
                    {
                        string[] listIdDelete = listId.Split(",");
                        if (listIdDelete.Length > 0)
                        {
                            foreach (string id in listIdDelete)
                            {
                                submitInfoCustomerWin = _businessContext.submitInfoCustomerWins.Find(id.Equals("") ? 0 : long.Parse(id));
                                if (submitInfoCustomerWin != null)
                                {

                                if (fid.ToString().Equals(SysFrameworks.Constant.C_F_APPROVE))
                                {
                                    submitInfoCustomerWin.CustomerStatus = SysFrameworks.Constant.C_STATUS_CUS_DUYET;
                                }
                                else if (fid.ToString().Equals(SysFrameworks.Constant.C_F_REJECT))
                                {
                                    submitInfoCustomerWin.CustomerStatus = SysFrameworks.Constant.C_STATUS_CUS_TU_CHOI;
                                }
                                else if (fid.ToString().Equals(SysFrameworks.Constant.C_F_REWARD))
                                {
                                    submitInfoCustomerWin.CustomerStatus = SysFrameworks.Constant.C_STATUS_CUS_TRA_THUONG;
                                }
                                    submitInfoCustomerWin.ModifyDate = DateTime.Now;
                                    submitInfoCustomerWin.ModifiedBy = (int)HttpContext.Session.GetInt32("s_user_id").Value;
                                    _businessContext.submitInfoCustomerWins.Update(submitInfoCustomerWin);
                                }
                            }
                        _businessContext.SaveChanges();
                        
                        }
                        else
                        {
                            response.success = "N";
                            response.message = "Hãy chọn dòng để thay đổi trạng thái!";
                        }
                    }
                    else
                    {
                        response.success = "N";
                        response.message = "Hãy chọn dòng để thay đổi trạng thái!";
                    }
                }
                catch (Exception ex)
                {
                    response.success = "N";
                    response.message = ex.Message;
                }
            
            return Json(response);
        }

        public IActionResult EditOrViewFileCustomerWin(EditFileCustomerWinDTO editFileCustomerWinDTO)
        {
            V_B_SUBMIT_CUSTOMER_WIN v_B_SUBMIT_CUSTOMER_WIN = _businessContext.V_B_SUBMIT_CUSTOMER_WIN.Find(editFileCustomerWinDTO.ID);
            v_B_SUBMIT_CUSTOMER_WIN.CopyProperties(editFileCustomerWinDTO);
            editFileCustomerWinDTO.ASC_PROCESS_DEADLINE = v_B_SUBMIT_CUSTOMER_WIN.ASC_PROCESS_DEADLINE == null ? null : DateTime.Parse(v_B_SUBMIT_CUSTOMER_WIN.ASC_PROCESS_DEADLINE.ToString()).ToString("dd/MM/yyyy");
            ViewData["Title"] = GetFunctionName(int.Parse(HttpContext.Session.GetString("fid")));
            ViewData["infoCustomerWin"] = editFileCustomerWinDTO;
            List<SelectListItem> ASCList = _masterContext.aSCMasters.Where(p => p.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC))
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.ASC_NAME
                    }).ToList();
            ASCList.Insert(0, new SelectListItem("", ""));
            ViewBag.ListASC = ASCList;

            //Status ASC
            const long C_TYPE_ASC_STATUS = 3;
            List<SelectListItem> lsASC_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_ASC_STATUS)
                    .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.Status
                    }).ToList();
            lsASC_STATUS.Insert(0, new SelectListItem("", ""));
            ViewBag.lsASC_STATUS = lsASC_STATUS;

            //Status PAN
            const long C_TYPE_PAN_STATUS = 1;
            const long C_ID_PAN_STATUS_DA_NOP_HS = 8;
            List<SelectListItem> lsPAN_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_PAN_STATUS || p.Id == C_ID_PAN_STATUS_DA_NOP_HS)
                    .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.Status
                    }).ToList();
            lsPAN_STATUS.Insert(0, new SelectListItem("", ""));
            ViewBag.lsPAN_STATUS = lsPAN_STATUS;


            //Cus status
            const long C_TYPE_CUSTOMER_STATUS = 2;
            List<SelectListItem> lsCUS_STATUS = _masterContext.mStatuses.Where(p => p.TypeStatus == C_TYPE_CUSTOMER_STATUS)
                    .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.Status
                    }).ToList();
            lsCUS_STATUS.Insert(0, new SelectListItem("", ""));
            ViewBag.lsCUS_STATUS = lsCUS_STATUS;

            List<SelectListItem> lsCUS_REJECT = _masterContext.M_CUSTOMER_REJECT
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.REASON
                    }).ToList();

            ViewBag.lsCUS_REJECT = lsCUS_REJECT;

            List<B_STEP_CUSTOMER_SUBMIT> b_STEP_CUSTOMER_SUBMITs = _businessContext.b_STEP_CUSTOMER_SUBMITs.Where(p => p.CUSTOMER_SUBMIT_ID == editFileCustomerWinDTO.ID).OrderBy(s => s.STEP_ID).ToList<B_STEP_CUSTOMER_SUBMIT>();

            foreach(B_STEP_CUSTOMER_SUBMIT item in b_STEP_CUSTOMER_SUBMITs)
            {
                switch (item.STEP_ID)
                {
                    case 1:
                        editFileCustomerWinDTO.NOTE_ERROR1 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR1 = true;
                        break;
                    case 2:
                        editFileCustomerWinDTO.NOTE_ERROR2 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR2 = true;
                        break;
                    case 3:
                        editFileCustomerWinDTO.NOTE_ERROR3 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR3 = true;
                        break;
                    case 4:
                        editFileCustomerWinDTO.NOTE_ERROR4 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR4 = true;
                        break;
                    case 5:
                        editFileCustomerWinDTO.NOTE_ERROR5 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR5 = true;
                        break;
                    case 6:
                        editFileCustomerWinDTO.NOTE_ERROR6 = item.NOTE_ERROR;
                        editFileCustomerWinDTO.IS_ERROR6 = true;
                        break;
                }
            }
            //editFileCustomerWinDTO.NOTE_ERROR1 = 

            editFileCustomerWinDTO.fid = HttpContext.Session.GetString("fid");
            ViewBag.funcId = HttpContext.Session.GetString("fid");
            return View("EditFileCustomerWin", editFileCustomerWinDTO);
        }
        public IActionResult SaveFileCustomerWin(EditFileCustomerWinDTO editFileCustomerWinDTO)
        {
            try
            {
                SubmitInfoCustomerWin submitInfoCustomerWin = _masterContext.SubmitInfoCustomerWin.Find(editFileCustomerWinDTO.ID);
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC || editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ADMIN)
                {
                    submitInfoCustomerWin.AscId = editFileCustomerWinDTO.ASC_ID;
                    submitInfoCustomerWin.Note = editFileCustomerWinDTO.NOTE;
                }

                if (editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ASC || editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ADMIN || editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ASSIGN_TO_ASC)
                {
                    submitInfoCustomerWin.NoteOfAsc = editFileCustomerWinDTO.NOTE_OF_ASC;
                    if (editFileCustomerWinDTO.ASC_PROCESS_DEADLINE != null)
                    {
                        submitInfoCustomerWin.ASC_PROCESS_DEADLINE = DateTime.ParseExact(editFileCustomerWinDTO.ASC_PROCESS_DEADLINE, "dd/MM/yyyy", provider);
                    }

                    //Kiem tra anh asc up len

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string foldersName = Path.Combine(wwwRootPath, "DocumentsSubmit");
                    string fileNameFileASCUpdate = "";
                    var formFileASCUpdate = editFileCustomerWinDTO.FILE_UPLOAD_OF_ASC;
                    if (formFileASCUpdate != null)
                    {
                        foreach (var formFile in formFileASCUpdate)
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
                                fileNameFileASCUpdate += (fileNameFileASCUpdate == "") ? fileNameServer : (";" + fileNameServer);
                            }
                        }
                    }
                    submitInfoCustomerWin.FileUploadOfAsc = String.IsNullOrEmpty(fileNameFileASCUpdate) ? submitInfoCustomerWin.FileUploadOfAsc : (String.IsNullOrEmpty(submitInfoCustomerWin.FileUploadOfAsc) ? fileNameFileASCUpdate : submitInfoCustomerWin.FileUploadOfAsc + ";" + fileNameFileASCUpdate);
                    submitInfoCustomerWin.ASC_STATUS = editFileCustomerWinDTO.ASC_STATUS;
                }

                if (editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_AGENCY || editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ADMIN)
                {
                    List<B_STEP_CUSTOMER_SUBMIT> b_STEP_CUSTOMER_SUBMITs = _businessContext.b_STEP_CUSTOMER_SUBMITs.Where(p => p.CUSTOMER_SUBMIT_ID == editFileCustomerWinDTO.ID).ToList<B_STEP_CUSTOMER_SUBMIT>();
                    if (b_STEP_CUSTOMER_SUBMITs != null && b_STEP_CUSTOMER_SUBMITs.Count > 0)
                    _businessContext.b_STEP_CUSTOMER_SUBMITs.RemoveRange(b_STEP_CUSTOMER_SUBMITs);
                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR1))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 1;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR1;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }

                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR2))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 2;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR2;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }

                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR3))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 3;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR3;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }

                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR4))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 4;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR4;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }

                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR5))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 5;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR5;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }

                    if (!string.IsNullOrEmpty(editFileCustomerWinDTO.NOTE_ERROR6))
                    {
                        B_STEP_CUSTOMER_SUBMIT b_STEP_CUSTOMER_SUBMIT = new B_STEP_CUSTOMER_SUBMIT();
                        b_STEP_CUSTOMER_SUBMIT.CUSTOMER_SUBMIT_ID = editFileCustomerWinDTO.ID;
                        b_STEP_CUSTOMER_SUBMIT.STEP_ID = 6;
                        b_STEP_CUSTOMER_SUBMIT.NOTE_ERROR = editFileCustomerWinDTO.NOTE_ERROR6;
                        _businessContext.b_STEP_CUSTOMER_SUBMITs.Add(b_STEP_CUSTOMER_SUBMIT);
                    }
                    
                    //Agency
                    submitInfoCustomerWin.NOTE_OF_AGENCY = editFileCustomerWinDTO.NOTE_OF_AGENCY;
                    submitInfoCustomerWin.PanStatus = editFileCustomerWinDTO.PAN_STATUS;

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string foldersName = Path.Combine(wwwRootPath, "DocumentsSubmit");
                    string fileNameileOfAgency = "";
                    var formFileOfAgency = editFileCustomerWinDTO.FILE_OF_AGENCY;
                    if (formFileOfAgency != null)
                    {
                        foreach (var formFile in formFileOfAgency)
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
                                fileNameileOfAgency += (fileNameileOfAgency == "") ? fileNameServer : (";" + fileNameServer);
                            }
                        }
                    }
                    submitInfoCustomerWin.FILE_OF_AGENCY = String.IsNullOrEmpty(fileNameileOfAgency) ? submitInfoCustomerWin.FILE_OF_AGENCY : fileNameileOfAgency;
                }
                if (editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_CALL_CENTER || editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ADMIN)
                {
                    //CALL_CENTER
                    submitInfoCustomerWin.NOTE_OF_CALL_CENTER = editFileCustomerWinDTO.NOTE_OF_CALL_CENTER;

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string foldersName = Path.Combine(wwwRootPath, "DocumentsSubmit");
                    string fileNameileOfCallCenter = "";
                    var formFileOfCallCenter = editFileCustomerWinDTO.FILE_OF_CALL_CENTER;
                    if (formFileOfCallCenter != null)
                    {
                        foreach (var formFile in formFileOfCallCenter)
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
                                fileNameileOfCallCenter += (fileNameileOfCallCenter == "") ? fileNameServer : (";" + fileNameServer);
                            }
                        }
                    }
                    submitInfoCustomerWin.FILE_OF_CALL_CENTER = String.IsNullOrEmpty(fileNameileOfCallCenter) ? submitInfoCustomerWin.FILE_OF_CALL_CENTER : fileNameileOfCallCenter;

                }
                if (editFileCustomerWinDTO.fid == SysFrameworks.Constant.C_ROLE_ADMIN)
                {
                    //admin
                    const long C_CUSTOMER_STATUS_REJECT = 11;
                    submitInfoCustomerWin.CustomerStatus = editFileCustomerWinDTO.CUSTOMER_STATUS;

                    submitInfoCustomerWin.NOTE_CUS_REJECT_ID = editFileCustomerWinDTO.NOTE_CUS_REJECT_ID;
                    submitInfoCustomerWin.NOTE_OF_ADMIN = editFileCustomerWinDTO.NOTE_OF_ADMIN;
                    if (editFileCustomerWinDTO.NOTE_CUS_REJECT != null && editFileCustomerWinDTO.CUSTOMER_STATUS == C_CUSTOMER_STATUS_REJECT)
                    {

                        submitInfoCustomerWin.NOTE_CUS_REJECT = editFileCustomerWinDTO.NOTE_CUS_REJECT;
                        submitInfoCustomerWin.REJECT_DATE = DateTime.Now;
                    }
                    else
                    {
                        submitInfoCustomerWin.NOTE_CUS_REJECT = "";
                    }
                }

                _masterContext.SubmitInfoCustomerWin.Update(submitInfoCustomerWin);


                _masterContext.SaveChanges();
                _businessContext.SaveChanges();
                FileCustomerWinDTO fileCustomerWinDTO = new FileCustomerWinDTO();
                fileCustomerWinDTO.fid = HttpContext.Session.GetString("fid");
                return RedirectToAction("ViewListWinner", "Business", fileCustomerWinDTO);
            }
            catch (Exception ex)
            {
                return View("EditFileCustomerWin", editFileCustomerWinDTO);
            }

        }
        public IActionResult BackToViewFileCustomerWin()
        {
            FileCustomerWinDTO fileCustomerWinDTO = new FileCustomerWinDTO();
            fileCustomerWinDTO.fid = HttpContext.Session.GetString("fid");
            return RedirectToAction("ViewFileCustomerWin", "Business", fileCustomerWinDTO);
        }
        public FileResult DownloadFileCustomerWin(long id)
        {
            try
            {
                string tempOutput = "";

                SubmitInfoCustomerWin submitInfoCustomerWin = _businessContext.submitInfoCustomerWins.Find(id);
                string fileName = submitInfoCustomerWin.FullName + DateTime.Now.Ticks.ToString() + ".zip";
                string wwwRootPath = _hostEnvironment.WebRootPath;
                tempOutput = Path.Combine(Constant.LUCKY_DRAW_FILE_PATH, fileName);
                using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
                {
                    zipOutputStream.SetLevel(3);
                    byte[] buffer = new byte[4096];
                    var imageList = new List<string>();
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileIDFront)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FileIDFront)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileIDBackside)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FileIDBackside)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileInvoice)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FileInvoice)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileProduct)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FileProduct)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FILE_ID_FRONT_AUTHORIZATION)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FILE_ID_FRONT_AUTHORIZATION)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FILE_ID_BACKSIDE_AUTHORIZATION)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FILE_ID_BACKSIDE_AUTHORIZATION)));
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileAuthozizationLetter))
                    {
                        string[] arr = submitInfoCustomerWin.FileAuthozizationLetter.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }
                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileSMSCode))
                    {
                        string[] arr = submitInfoCustomerWin.FileSMSCode.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileSMSWin))
                    {
                        string[] arr = submitInfoCustomerWin.FileSMSWin.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileSerialNumber))
                    {
                        string[] arr = submitInfoCustomerWin.FileSerialNumber.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileUploadOfAsc))
                    {
                        string[] arr = submitInfoCustomerWin.FileUploadOfAsc.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FILE_OF_CALL_CENTER))
                    {
                        string[] arr = submitInfoCustomerWin.FILE_OF_CALL_CENTER.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    if (!string.IsNullOrEmpty(submitInfoCustomerWin.FILE_OF_AGENCY))
                    {
                        string[] arr = submitInfoCustomerWin.FILE_OF_AGENCY.Split(";");
                        foreach (string item in arr)
                        {
                            imageList.Add(Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", item)));
                        }
                    }

                    for (int i = 0; i < imageList.Count; ++i)
                    {
                        ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(imageList[i]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        zipOutputStream.PutNextEntry(entry);
                        using (FileStream fileStream = System.IO.File.OpenRead(imageList[i]))
                        {
                            int sourceByte;
                            do
                            {
                                sourceByte = fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, sourceByte);
                            } while (sourceByte > 0);
                        }
                    }

                    zipOutputStream.Finish();
                    zipOutputStream.Flush();
                    zipOutputStream.Close();
                }
                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);

                }
                if (finalResult == null || !finalResult.Any())
                {
                    throw new Exception("File not found!");
                }
                return File(finalResult, "application/zip", fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
            //string fullPath = "";
            //byte[] fileBytes = null;

        }

        public FileResult DownloadFileProduct()
        {
            try
            {
                string tempOutput = "";

                List<SubmitInfoCustomerWin> submitInfoCustomerWins = _businessContext.submitInfoCustomerWins.Where(p => p.CustomerStatus == Constant.C_STATUS_CUS_DUYET).ToList();

                    string fileName = DateTime.Now.Ticks.ToString() + ".zip";
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    tempOutput = Path.Combine(Constant.LUCKY_DRAW_FILE_PATH, fileName);
                    using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
                    {
                        zipOutputStream.SetLevel(3);
                        byte[] buffer = new byte[4096];
                        var imageList = new List<string>();
                    foreach (SubmitInfoCustomerWin submitInfoCustomerWin in submitInfoCustomerWins)
                    {
                        if (!String.IsNullOrEmpty(submitInfoCustomerWin.FileProduct))
                        {
                            BCustomerWin customerWin = _businessContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(submitInfoCustomerWin.PhoneNumber)).FirstOrDefault();
                            M_PROVINCE m_PROVINCE = _appRegisterContext.M_PROVINCE.Find(submitInfoCustomerWin.ProvinceId);
                            
                            string fileSrc = Path.Combine(wwwRootPath, Path.Combine("DocumentsSubmit", submitInfoCustomerWin.FileProduct));
                            string extension = Path.GetExtension(fileSrc);
                            string foldersName = Path.Combine(wwwRootPath, Constant.PATH_DOCUMENTS_EXPORT);
                            string fileDes = customerWin.LuckyCode + "_" + submitInfoCustomerWin.FullName + "_" + (customerWin.PrizeWin == 1 ? "IPhone" : "Hoàn Tiền")
                                + "_" + customerWin.ModelName + "_" + m_PROVINCE.PROVINCE_NAME + extension;
                            var filePath = Path.Combine(foldersName, fileDes);
                            if (!System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Copy(fileSrc, filePath);
                            }

                            if (!string.IsNullOrEmpty(submitInfoCustomerWin.FileProduct)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine(Constant.PATH_DOCUMENTS_EXPORT, submitInfoCustomerWin.FileProduct)));
                        }
                    }
                    for (int i = 0; i < imageList.Count; ++i)
                    {
                        ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(imageList[i]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        zipOutputStream.PutNextEntry(entry);
                        using (FileStream fileStream = System.IO.File.OpenRead(imageList[i]))
                        {
                            int sourceByte;
                            do
                            {
                                sourceByte = fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, sourceByte);
                            } while (sourceByte > 0);
                        }
                    }

                        zipOutputStream.Finish();
                        zipOutputStream.Flush();
                        zipOutputStream.Close();
                    
                }

                byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);

                }
                if (finalResult == null || !finalResult.Any())
                {
                    throw new Exception("File not found!");
                }
                return File(finalResult, "application/zip", fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
            //string fullPath = "";
            //byte[] fileBytes = null;

        }

        public IActionResult ExportExcel(FileCustomerWinDTO assginASCDTO)
        {
            try
            {
                S_USER s_USER = _masterContext.S_USER.Find((long)HttpContext.Session.GetInt32("s_user_id").Value);
                ASCMaster aSCMaster = _masterContext.aSCMasters.Where(p => p.ID == s_USER.ASC_ID).FirstOrDefault<ASCMaster>();

                if (aSCMaster.ASC_TYPE.Equals(SysFrameworks.Constant.C_ROLE_ASC)) // acc login là ASC
                {
                    assginASCDTO.AscId = aSCMaster.ID;
                    assginASCDTO.StatusId = 1;
                }
                if (s_USER.ROLES.Equals(SysFrameworks.Constant.C_ROLE_AGENCY))
                {
                    assginASCDTO.AGENCY_ID = s_USER.ID;
                }
                List<FileCustomerWinSearchDTO> assginASCDTOs = _businessContext.SearchFileCustomerWin(assginASCDTO, 1, 1000000);
                ExportExcel exportExcel = new ExportExcel();
                byte[] fileBytes = exportExcel.ExportExcelCustomerWin(assginASCDTOs);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Báo cáo dữ liệu.xlsx");
            }
            catch (Exception ex)
            {
                ViewBag.errName = ex.Message;
            }
            return View("ViewFileCustomerWin", assginASCDTO);
        }

        public IActionResult ViewInputFileCustomer()
        {
            return View("InputFileCustomer");
        }

        public JsonResult CheckFileCustomer(string PhoneNumber)
        {

            JsonResponse responseJson = new JsonResponse();
            try
            {
                if (PhoneNumber != null || PhoneNumber != "")
                {
                    BCustomerWin bCustomerWin = _businessContext.bCustomerWins.Where(p => p.PhoneNumber.Equals(SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()))).FirstOrDefault<BCustomerWin>();
                    if (bCustomerWin == null)
                    {
                        responseJson.success = "N";
                        responseJson.message = "Số điện thoại không hợp lệ!";
                    } else
                    {
                        HttpContext.Session.SetString("PhoneNumber", SysFrameworks.Extensions.ToNormalPhoneNumber(PhoneNumber.Trim()));
                        responseJson.success = "Y";
                    }
                }
                else
                {
                    responseJson.success = "N";
                    responseJson.message = "Chưa nhập số điện thoại!";
                }
            }
            catch (Exception ex)
            {

                responseJson.success = "N";
                responseJson.message = ex.Message;
            }
            return Json(responseJson);
        }
    }
}
