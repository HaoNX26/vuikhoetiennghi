using JoinReward.Models.DTO;
using JoinReward.Models.JoinReward;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PAN.Warranty.Models.Master;
using PAN.Warranty.Models.Master.JoinReward;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinReward.Models.DB
{
    public class AppRegisterContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private HttpContext _context => _httpContextAccessor.HttpContext;

        public AppRegisterContext(DbContextOptions<AppRegisterContext> options)
        : base(options)
        {
        }

        public DbSet<AppRegisterModel> appRegisterModels { get; set; }

        public DbSet<M_DISTRICT> M_DISTRICT { get; set; }

        public DbSet<M_PROVINCE> M_PROVINCE { get; set; }

        public DbSet<M_MODEL> M_MODEL { get; set; }

        public DbSet<M_PRODUCT_NAME> M_PRODUCT_NAME { get; set; }
        public DbSet<ExportCustomerRegisterDTO> exportCustomerRegisterDTOs { get; set; }

        public List<ExportCustomerRegisterDTO> SearchCustomerRegister(DateTime fromDate, DateTime toDate, long p_Cur_Page, long p_Page_Size)
        {
            // Initialization.  
            List<ExportCustomerRegisterDTO> lst;

            SqlParameter p_from_date = new SqlParameter("@p_from_date", fromDate);
            SqlParameter p_to_date = new SqlParameter("@p_to_date", toDate);
            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.exportCustomerRegisterDTOs.FromSqlRaw("p_export_customer_register$Search @p_from_date, @p_to_date, @p_cur_page, @p_page_size", p_from_date, p_to_date, p_cur_page, p_page_size).ToList<ExportCustomerRegisterDTO>();
            return lst;
        }
    }
}
