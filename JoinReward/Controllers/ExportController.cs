using ICSharpCode.SharpZipLib.Zip;
using JoinReward.Models.DB;
using JoinReward.Models.DTO;
using JoinReward.Models.JoinReward;
using JoinReward.Models.SubmitCustomerWin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PAN.Warranty.Models.Master;
using PAN.Warranty.Models.Master.JoinReward;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JoinReward.Controllers
{
    public class ExportController : BaseController
    {
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly AppRegisterContext _appRegisterContext;
        private readonly SubmitCustomerWinContext _submitCustomerWinContext;

        public ExportController(IHostingEnvironment hostingEnvironment, AppRegisterContext appRegisterContext, SubmitCustomerWinContext submitCustomerWinContext)
        {
            _hostEnvironment = hostingEnvironment;
            _appRegisterContext = appRegisterContext;
            _submitCustomerWinContext = submitCustomerWinContext;
        }

        public IActionResult SearchCustomerRegiser(SearchCustomerRegisterDTO searchCustomerRegisterDTO)
        {
            try
            {
                string s_role = HttpContext.Session.GetString("s_role");

                if (s_role != "1")
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime fromDate = (searchCustomerRegisterDTO.FromDate == null) ? DateTime.Now.AddDays(-1) : DateTime.ParseExact(searchCustomerRegisterDTO.FromDate, "dd/MM/yyyy", provider);
                    DateTime toDate = (searchCustomerRegisterDTO.ToDate == null) ? DateTime.Now : DateTime.ParseExact(searchCustomerRegisterDTO.ToDate, "dd/MM/yyyy", provider);
                    if (searchCustomerRegisterDTO.PAGE_SIZE <= 0) searchCustomerRegisterDTO.PAGE_SIZE = SysFrameworks.Constant.C_DEFAULT_PAGE_SIZE;
                    if (searchCustomerRegisterDTO.page <= 0) searchCustomerRegisterDTO.page = 1;
                    Models.Pagination pagination = new Models.Pagination()
                    {
                        PAGE_SIZE = searchCustomerRegisterDTO.PAGE_SIZE,
                        CUR_PAGE = searchCustomerRegisterDTO.page
                    };
                    List<ExportCustomerRegisterDTO> exportCustomerRegisterDTOs = _appRegisterContext.SearchCustomerRegister(fromDate, toDate, pagination.CUR_PAGE, pagination.PAGE_SIZE);
                    if (exportCustomerRegisterDTOs.Count > 0) pagination.TOTAL_RECORD = exportCustomerRegisterDTOs[0].NUM_OF_RECORD;
                    pagination.CONTROLLER = ControllerContext.ActionDescriptor.ControllerName;
                    pagination.ACTION = ControllerContext.ActionDescriptor.ActionName;
                    //L?y thu?c tính truy?n qua query string
                    DataCollections cols = new DataCollections();
                    cols.Add(DataTypes.NVarchar, "FromDate", FieldTypes.DefaultValue, searchCustomerRegisterDTO.FromDate, "");
                    cols.Add(DataTypes.NVarchar, "ToDate", FieldTypes.DefaultValue, searchCustomerRegisterDTO.ToDate, "");
                    pagination.GET_QUERY_STRING = SysFrameworks.Url.to_Get_Param(cols);
                    ViewData["pagination"] = pagination;
                    ViewData["grid_data"] = exportCustomerRegisterDTOs;
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message.ToString();
            }
            return View("ExportCustomerRegister");
        }

        public IActionResult ExportExcel(SearchCustomerRegisterDTO searchCustomerRegisterDTO)
        {
            try
            {
                List<ExportExcelCustomer> exportExcelCustomers = new List<ExportExcelCustomer>();
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime fromDate = (searchCustomerRegisterDTO.FromDate == null) ? DateTime.Now.AddDays(-1) : DateTime.ParseExact(searchCustomerRegisterDTO.FromDate, "dd/MM/yyyy", provider);
                DateTime toDate = (searchCustomerRegisterDTO.ToDate == null) ? DateTime.Now : DateTime.ParseExact(searchCustomerRegisterDTO.ToDate, "dd/MM/yyyy", provider);
                List<AppRegisterModel> appRegisterModels = _appRegisterContext.appRegisterModels.Where(p => p.CreatedDate >= fromDate && p.CreatedDate <= toDate).OrderByDescending(x => x.Id).ToList();
                if (appRegisterModels.Count > 0)
                {
                    foreach (AppRegisterModel appRegisterModel in appRegisterModels)
                    {
                        ExportExcelCustomer exportExcelCustomer = new ExportExcelCustomer();
                        M_PRODUCT_NAME m_PRODUCT_NAME = _appRegisterContext.M_PRODUCT_NAME.Find(appRegisterModel.ProductNameId);
                        M_PROVINCE m_PROVINCE = _appRegisterContext.M_PROVINCE.Find(appRegisterModel.ProvinceId);
                        M_MODEL m_MODEL = _appRegisterContext.M_MODEL.Find(appRegisterModel.ModelId);
                        M_DISTRICT m_DISTRICT = _appRegisterContext.M_DISTRICT.Find(appRegisterModel.DistrictsId);

                        exportExcelCustomer.FullName = appRegisterModel.FullName;
                        exportExcelCustomer.PhoneNumber = appRegisterModel.PhoneNumber;
                        exportExcelCustomer.IdentificationNumber = appRegisterModel.IdentificationNumber;
                        exportExcelCustomer.Address = appRegisterModel.Address;
                        exportExcelCustomer.ProductName = m_PRODUCT_NAME == null ? "" : m_PRODUCT_NAME.PRODUCT_NAME;
                        exportExcelCustomer.Model = m_MODEL == null ? "" : m_MODEL.MODEL_NAME;
                        exportExcelCustomer.Province = m_PROVINCE == null ? "" : m_PROVINCE.PROVINCE_NAME;
                        exportExcelCustomer.Districts = m_DISTRICT == null ? "" : m_DISTRICT.DISTRICT_NAME;
                        exportExcelCustomer.PurchaseDate = appRegisterModel.PurchaseDate;
                        exportExcelCustomer.CreatedDate = appRegisterModel.CreatedDate;
                        exportExcelCustomer.FileIDFront = "http://vuikhoetiennghi.vn/Documents/" + appRegisterModel.FileIDFrontPath;
                        exportExcelCustomer.FileIDBackside = "http://vuikhoetiennghi.vn/Documents/" + appRegisterModel.FileIDBacksidePath;
                        exportExcelCustomer.FileWarranty = "http://vuikhoetiennghi.vn/Documents/" + appRegisterModel.FileWarrantyPath;
                        exportExcelCustomer.FileProduct = "http://vuikhoetiennghi.vn/Documents/" + appRegisterModel.FileProductPath;
                        exportExcelCustomers.Add(exportExcelCustomer);
                    }
                }
                ExportExcel exportExcel = new ExportExcel();
                byte[] fileBytes = exportExcel.DocumentTemExportExcel(exportExcelCustomers);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Báo cáo dữ liệu.xlsx");
            }
            catch (Exception ex)
            {
                ViewBag.errName = ex.Message;
            }
            return View("ExportExcel", searchCustomerRegisterDTO);
        }

        public FileResult DownloadFile(long id, long type)
        {
            //string fullPath = "";
            //byte[] fileBytes = null;
            string tempOutput = "";
            
            AppRegisterModel appRegisterModel = _appRegisterContext.appRegisterModels.Find(id);
            string fileName = appRegisterModel.FullName + DateTime.Now.Ticks.ToString() + ".zip";
            string wwwRootPath = _hostEnvironment.WebRootPath;
            tempOutput = Path.Combine(Constant.LUCKY_DRAW_FILE_PATH, fileName);
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                zipOutputStream.SetLevel(3);
                byte[] buffer = new byte[4096];
                var imageList = new List<string>();
                if (!string.IsNullOrEmpty(appRegisterModel.FileIDFrontPath)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileIDFrontPath)));
                if (!string.IsNullOrEmpty(appRegisterModel.FileIDFrontPath)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileIDBacksidePath)));
                if (!string.IsNullOrEmpty(appRegisterModel.FileIDFrontPath)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileWarrantyPath)));
                if (!string.IsNullOrEmpty(appRegisterModel.FileIDFrontPath)) imageList.Add(Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileProductPath)));

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
            //switch (type)
            //{
            //    case 1:
            //        fullPath = Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileIDFrontPath));
            //        fileBytes = System.IO.File.ReadAllBytes(fullPath);
            //        fileName = appRegisterModel.FileIDFrontPath;
            //        break;
            //    case 2:
            //        fullPath = Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileIDBacksidePath));
            //        fileBytes = System.IO.File.ReadAllBytes(fullPath);
            //        fileName = appRegisterModel.FileIDBacksidePath;
            //        break;
            //    case 3:
            //        fullPath = Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileWarrantyPath));
            //        fileBytes = System.IO.File.ReadAllBytes(fullPath);
            //        fileName = appRegisterModel.FileWarrantyPath;
            //        break;
            //    case 4:
            //        fullPath = Path.Combine(wwwRootPath, Path.Combine("Documents", appRegisterModel.FileProductPath));
            //        fileBytes = System.IO.File.ReadAllBytes(fullPath);
            //        fileName = appRegisterModel.FileProductPath;
            //        break;
            //}
            return File(finalResult, "application/zip", fileName);
        }



    }
}
