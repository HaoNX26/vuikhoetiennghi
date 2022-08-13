using JoinReward.Common;
using JoinReward.Models;
using JoinReward.Models.DB;
using JoinReward.Models.JoinReward;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using JoinReward.Models;
using PAN.Warranty.Models.Master;
using PAN.Warranty.Models.Master.JoinReward;
using SysFrameworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JoinReward.Models.DTO;
using JoinReward.Models.Master;

namespace JoinReward.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly AppRegisterContext _appRegisterContext;
        private readonly MasterContext _masterContext;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment, AppRegisterContext appRegisterContext, MasterContext masterContext)
        {
            _logger = logger;
            _hostEnvironment = hostingEnvironment;
            _appRegisterContext = appRegisterContext;
            _masterContext = masterContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("dangkythamgia")]
        public IActionResult FormRegister()
        {
            List<SelectListItem> provinces = _appRegisterContext.M_PROVINCE
              .Select(a => new SelectListItem()
               {
                   Value = a.ID.ToString(),
                   Text = a.PROVINCE_NAME
               }).ToList();
            List<SelectListItem> models = _appRegisterContext.M_MODEL
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.MODEL_NAME
                }).ToList();
            List<SelectListItem> products = _appRegisterContext.M_PRODUCT_NAME
            .Select(a => new SelectListItem()
            {
                Value = a.ID.ToString(),
                Text = a.PRODUCT_NAME
            }).ToList();
            List<SelectListItem> districts = _appRegisterContext.M_DISTRICT
            .Select(a => new SelectListItem()
            {
                Value = a.ID.ToString(),
                Text = a.DISTRICT_NAME
            }).ToList();
            provinces.Insert(0, new SelectListItem("", ""));
            models.Insert(0, new SelectListItem("", ""));
            products.Insert(0, new SelectListItem("", ""));
            districts.Insert(0, new SelectListItem("", ""));
            ViewBag.listProvince = provinces;
            ViewBag.listModel = models;
            ViewBag.listProduct = products;
            ViewBag.listDistrict = districts;
            return View("FormRegister");
            //return View("ExpiresRegister");
        }

        [Route("guithongtin")]
        public IActionResult SubmitFormRegister(AppRegisterDTO appRegisterDTO)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string captchaResponse =
                        Request.Form["g-Recaptcha-Response"];
                ReCaptchaValidationResult reCaptchaValidationResult = ReCaptchaValidator.IsValid(captchaResponse);
                //string capchar = HttpContext.Session.GetString("CaptchaCode");

                bool valid = true;
                if (appRegisterDTO.FullName == null)
                { ViewBag.errFullName = "Họ và tên không được để trống!"; valid = false; }
                if (appRegisterDTO.PhoneNumber == null)
                {
                    ViewBag.errPhoneNumber = "Số điện thoại không được để trống"; valid = false;
                }
                else if (!Common.CString.ValidatePhoneNumber(appRegisterDTO.PhoneNumber))
                {
                    ViewBag.errPhoneNumber = "Số điện thoại không đúng định dạng"; valid = false;
                }
                if (appRegisterDTO.ProductNameId == 0)
                {
                    ViewBag.errProductName = "Tên sản phẩm không được để trống"; valid = false;
                }
                if (appRegisterDTO.ModelId == 0)
                {
                    ViewBag.errModel = "Kiểu máy không được để trống"; valid = false;
                }
                if (appRegisterDTO.ProvinceId == 0)
                {
                    ViewBag.errProvince = "Tên tỉnh, thành phố không được để trống"; valid = false;
                }
                if (appRegisterDTO.DistrictsId == 0)
                {
                    ViewBag.errDistricts = "Tên quận, huyện không được để trống"; valid = false;
                }
                if (appRegisterDTO.PurchaseDate == null)
                {
                    ViewBag.errPurchaseDate = "Ngày mua hàng không được để trống"; valid = false;
                } 
                else if (DateTime.ParseExact(appRegisterDTO.PurchaseDate, "dd/MM/yyyy", provider) > DateTime.Now)
                {
                    ViewBag.errPurchaseDate = "Ngày mua hàng không được sau ngày hiện tại"; valid = false;
                }
                if (appRegisterDTO.IdentificationNumber == null)
                {
                    ViewBag.errIdentificationNumber = "Sô CMTND hoặc CCCD không được để trống"; valid = false;
                }
                if (appRegisterDTO.Address == null)
                {
                    ViewBag.errAddress = "Địa chỉ chứa sản phẩm không được để trống"; valid = false;
                }
                if (appRegisterDTO.FileIDFront == null)
                {
                    ViewBag.errFileIDFront = "Ảnh chụp CMTND, CCCD mặt trước không được để trống"; valid = false;
                } else if (!Common.CString.ValidateFileName(Path.GetExtension(appRegisterDTO.FileIDFront.FileName)))
                {
                    ViewBag.errFileIDFront = "Ảnh chụp CMTND, CCCD mặt trước không đúng định dạng"; valid = false;
                }
                if (appRegisterDTO.FileIDBackside == null)
                {
                    ViewBag.errFileIDBackside = "Ảnh chụp CMTND, CCCD mặt sau không được để trống"; valid = false;
                } else if (!Common.CString.ValidateFileName(Path.GetExtension(appRegisterDTO.FileIDBackside.FileName)))
                {
                    ViewBag.errFileIDBackside = "Ảnh chụp CMTND, CCCD mặt sau không đúng định dạng"; valid = false;
                }
                if (appRegisterDTO.FileProduct == null)
                {
                    ViewBag.errFileProduct = "Ảnh chụp sản phẩm không được để trống"; valid = false;
                }
                else if (!Common.CString.ValidateFileName(Path.GetExtension(appRegisterDTO.FileProduct.FileName)))
                {
                    ViewBag.errFileProduct = "Ảnh chụp sản phẩm không đúng định dạng"; valid = false;
                }
                if (appRegisterDTO.FileWarranty == null)
                {
                    ViewBag.errFileWarranty = "Ảnh chụp phiếu bảo hành không được để trống"; valid = false;
                }
                else if (!Common.CString.ValidateFileName(Path.GetExtension(appRegisterDTO.FileWarranty.FileName)))
                {
                    ViewBag.errFileWarranty = "Ảnh chụp phiếu bảo hành không đúng định dạng"; valid = false;
                }
                if (valid)
                {
                    AppRegisterModel appRegisterModel = new AppRegisterModel();
                    appRegisterModel.FullName = appRegisterDTO.FullName;
                    appRegisterModel.PhoneNumber = appRegisterDTO.PhoneNumber;
                    appRegisterModel.ProductNameId = appRegisterDTO.ProductNameId;
                    appRegisterModel.ModelId = appRegisterDTO.ModelId;
                    appRegisterModel.PurchaseDate = DateTime.ParseExact(appRegisterDTO.PurchaseDate, "dd/MM/yyyy", provider);
                    appRegisterModel.IdentificationNumber = appRegisterDTO.IdentificationNumber;
                    appRegisterModel.Address = appRegisterDTO.Address;
                    appRegisterModel.ProvinceId = appRegisterDTO.ProvinceId;
                    appRegisterModel.DistrictsId = appRegisterDTO.DistrictsId;
                    appRegisterModel.CreatedDate = DateTime.Now;

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string foldersName = Path.Combine(wwwRootPath, "Documents");
                    var formFileFront = appRegisterDTO.FileIDFront;
                    var formFileBackside = appRegisterDTO.FileIDBackside;
                    var formFileWarranty = appRegisterDTO.FileWarranty;
                    var formFileProduct = appRegisterDTO.FileProduct;
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
                        appRegisterModel.FileIDFrontPath = fileNameServer;
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
                        appRegisterModel.FileIDBacksidePath = fileNameServer;
                    }

                    if (formFileWarranty != null)
                    {
                        string originalFileName = Path.GetFileNameWithoutExtension(formFileWarranty.FileName);
                        string extension = Path.GetExtension(formFileWarranty.FileName);
                        string fileNameServer = Common.CString.RemoveSpecialCharacters(Common.CString.RemoveUnicodeFile(originalFileName) + DateTime.Now.ToString("yymmssfff") + extension);

                        var filePath = Path.Combine(foldersName, fileNameServer);
                        using (var filestream = new FileStream(filePath, FileMode.Create))
                        {
                            formFileWarranty.CopyTo(filestream);
                        }
                        appRegisterModel.FileWarrantyPath = fileNameServer;
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
                        appRegisterModel.FileProductPath = fileNameServer;
                    }
                    _appRegisterContext.appRegisterModels.Add(appRegisterModel);
                    _appRegisterContext.SaveChanges();
                    appRegisterDTO.Id = appRegisterModel.Id;
                }
                else
                {
                    List<SelectListItem> provinces = _appRegisterContext.M_PROVINCE
                      .Select(a => new SelectListItem()
                      {
                          Value = a.ID.ToString(),
                          Text = a.PROVINCE_NAME
                      }).ToList();
                    List<SelectListItem> models = _appRegisterContext.M_MODEL.Where(p => p.PRODUCT_ID == appRegisterDTO.ProductNameId)
                        .Select(a => new SelectListItem()
                        {
                            Value = a.ID.ToString(),
                            Text = a.MODEL_NAME
                        }).ToList();
                    List<SelectListItem> products = _appRegisterContext.M_PRODUCT_NAME
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.PRODUCT_NAME
                    }).ToList();
                    List<SelectListItem> districts = _appRegisterContext.M_DISTRICT.Where(p=> p.PROVINCE_ID == appRegisterDTO.ProvinceId)
                    .Select(a => new SelectListItem()
                    {
                        Value = a.ID.ToString(),
                        Text = a.DISTRICT_NAME
                    }).ToList();
                    provinces.Insert(0, new SelectListItem("", ""));
                    models.Insert(0, new SelectListItem("", ""));
                    products.Insert(0, new SelectListItem("", ""));
                    districts.Insert(0, new SelectListItem("", ""));
                    ViewBag.listProvince = provinces;
                    ViewBag.listModel = models;
                    ViewBag.listProduct = products;
                    ViewBag.listDistrict = districts;
                    return View("FormRegister", appRegisterDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("FormRegister", appRegisterDTO);
            }
            return Redirect("/guithanhcong");
        }

        public JsonResult ProvincesByRegions(string regions)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                List<M_PROVINCE> provinces = _appRegisterContext.M_PROVINCE.ToList();
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                foreach (M_PROVINCE province in provinces)
                {
                    stringBuilder.Append(string.Format("<option value='{0}'>{1}</option>", province.ID, province.PROVINCE_NAME));
                }
                response.success = "Y";
                response.data = provinces.ToArray();
                response.data_string = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }
            return Json(response);
        }
        public JsonResult DistrictsByProvince(long province)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                List<M_DISTRICT> districts = _appRegisterContext.M_DISTRICT.Where(p => p.PROVINCE_ID == province).ToList();
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("<option value=''></option>");
                foreach (M_DISTRICT obj in districts)
                {
                    stringBuilder.Append(string.Format("<option value='{0}'>{1}</option>", obj.ID, obj.DISTRICT_NAME));
                }
                response.success = "Y";
                response.data = districts.ToArray();
                response.data_string = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }
            return Json(response);
        }

        public JsonResult ModelByProduct(long product)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                List<M_MODEL> districts = _appRegisterContext.M_MODEL.Where(p => p.PRODUCT_ID == product).ToList();
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("<option value=''></option>");
                foreach (M_MODEL obj in districts)
                {
                    stringBuilder.Append(string.Format("<option value='{0}'>{1}</option>", obj.ID, obj.MODEL_NAME));
                }
                response.success = "Y";
                response.data = districts.ToArray();
                response.data_string = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }
            return Json(response);
        }

        public JsonResult WardByDistrict(long district)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                List<MWard> wards = _masterContext.mWards.Where(p => p.DistrictId == district).ToList();
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("<option value=''></option>");
                foreach (MWard obj in wards)
                {
                    stringBuilder.Append(string.Format("<option value='{0}'>{1}</option>", obj.Id, obj.WardName));
                }
                response.success = "Y";
                response.data = wards.ToArray();
                response.data_string = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }
            return Json(response);
        }

        public IActionResult FormCustomerView(AppRegisterDTO appRegisterDTO)
        {
            CustomerView customerView = new CustomerView();
            List<SelectListItem> provinces = _appRegisterContext.M_PROVINCE
              .Select(a => new SelectListItem()
              {
                  Value = a.ID.ToString(),
                  Text = a.PROVINCE_NAME
              }).ToList();
            List<SelectListItem> models = _appRegisterContext.M_MODEL
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.MODEL_NAME
                }).ToList();
            List<SelectListItem> products = _appRegisterContext.M_PRODUCT_NAME
            .Select(a => new SelectListItem()
            {
                Value = a.ID.ToString(),
                Text = a.PRODUCT_NAME
            }).ToList();
            List<SelectListItem> districts = _appRegisterContext.M_DISTRICT
            .Select(a => new SelectListItem()
            {
                Value = a.ID.ToString(),
                Text = a.DISTRICT_NAME
            }).ToList();
            provinces.Insert(0, new SelectListItem("", ""));
            models.Insert(0, new SelectListItem("", ""));
            products.Insert(0, new SelectListItem("", ""));
            districts.Insert(0, new SelectListItem("", ""));
            ViewBag.listProvince = provinces;
            ViewBag.listModel = models;
            ViewBag.listProduct = products;
            ViewBag.listDistrict = districts;
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string foldersName = Path.Combine(wwwRootPath, "Documents");
            AppRegisterModel appRegisterModel = _appRegisterContext.appRegisterModels.Find(appRegisterDTO.Id);
            customerView.DistrictsId = appRegisterModel.DistrictsId;
            customerView.Address = appRegisterModel.Address;
            customerView.FullName = appRegisterModel.FullName;
            customerView.IdentificationNumber = appRegisterModel.IdentificationNumber;
            customerView.PurchaseDate = appRegisterModel.PurchaseDate.ToString("dd/MM/yyyy");
            customerView.ProductNameId = appRegisterModel.ProductNameId;
            customerView.ProvinceId = appRegisterModel.ProvinceId;
            customerView.PhoneNumber = appRegisterModel.PhoneNumber;
            customerView.ModelId = appRegisterModel.ModelId;
            customerView.FileWarranty = Path.Combine(foldersName, appRegisterModel.FileWarrantyPath);
            customerView.FileIDFront = Path.Combine(foldersName, appRegisterModel.FileIDFrontPath);
            customerView.FileIDBackside = Path.Combine(foldersName, appRegisterModel.FileIDBacksidePath);
            ViewBag.pathFileWarranty = "/Documents/" + appRegisterModel.FileWarrantyPath;
            ViewBag.pathFrontPath = "/Documents/" + appRegisterModel.FileIDFrontPath;
            ViewBag.pathBacksidePath = "/Documents/" + appRegisterModel.FileIDBacksidePath;
            return View("FormCustomerView", customerView);
        }

        public IActionResult FormExportExcel()
        {
            return View("ExportExcel");
        }

        public IActionResult ExportExcel(ExportDto exportDto)
        {
            try
            {
                if ("admin".Equals(exportDto.UserName) && "Abc@123".Equals(exportDto.Password))
                {
                    List<ExportExcelCustomer> exportExcelCustomers = new List<ExportExcelCustomer>();

                    List<AppRegisterModel> appRegisterModels = _appRegisterContext.appRegisterModels.ToList();
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
                else
                {
                    ViewBag.errName = "Tài khoản hoặc mật khẩu không đúng!";
                }
            }catch (Exception ex)
            {
                ViewBag.errName = ex.Message;
            }
            return View("ExportExcel", exportDto);
        }

        public JsonResult ValidateCapcha(string captchaResponse)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                //string captchaResponse = Request.Form["g-Recaptcha-Response"];
                ReCaptchaValidationResult reCaptchaValidationResult = ReCaptchaValidator.IsValid(captchaResponse);
                if (!reCaptchaValidationResult.Success)
                {
                    response.data = "invalid";
                }
                else
                {
                    response.data = "valid";
                }
                response.success = "Y";
            } catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }

            return Json(response);
        }

        public JsonResult ValidateDateFuture(string purchaseDate)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (DateTime.ParseExact(purchaseDate, "dd/MM/yyyy", provider) > DateTime.Now)
                {
                    response.data = "invalid";
                }
                else
                {
                    response.data = "valid";
                }
                response.success = "Y";
            }
            catch (Exception ex)
            {
                response.success = "N";
                response.message = ex.Message;
            }

            return Json(response);
        }
    }
}
