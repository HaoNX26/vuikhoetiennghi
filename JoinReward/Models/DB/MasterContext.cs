using JoinReward.Models.DTO;
using JoinReward.Models.Master;
using JoinReward.Models.SubmitCustomerWin;
using JoinReward.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JoinReward.Models.DB
{
    public class MasterContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private HttpContext _context => _httpContextAccessor.HttpContext;

        public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
        {
        }

        public DbSet<ASCMaster> aSCMasters { get; set; }

        public DbSet<ASCMasterDTO> aSCMasterDTOs { get; set; }
        public DbSet<FileCustomerWinSearchDTO> assignAscSearchDTOs { get; set; }
        public DbSet<GetInfoCustomerWinDTO> getInfoCustomerWinDTOs { get; set; }
        public DbSet<MBank> mBanks { get; set; }

        public DbSet<S_USER> S_USER { get; set; }
        public DbSet<MStatus> mStatuses { get; set; }
        public DbSet<MWard> mWards { get; set; }
        public DbSet<M_OTP_DTO> M_OTP_DTO { get; set; }
        public DbSet<M_OTP> M_OTP { get; set; }
        public DbSet<SubmitInfoCustomerWin> SubmitInfoCustomerWin { get; set; }

        public DbSet<M_CUSTOMER_REJECT> M_CUSTOMER_REJECT { get; set; }
        public List<ASCMasterDTO> SearchAsc(string p_Keyword, long p_Cur_Page, long p_Page_Size)
        {
            // Initialization.  
            List<ASCMasterDTO> lst;

            SqlParameter p_keyword = new SqlParameter("@p_keyword", (p_Keyword == null) ? "" : p_Keyword);
            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.aSCMasterDTOs.FromSqlRaw("p_ASC_master$SEARCH @p_keyword, @p_cur_page, @p_page_size", p_keyword, p_cur_page, p_page_size).ToList<ASCMasterDTO>();
            return lst;
        }

        public List<M_OTP_DTO> SearchOTP(string p_Keyword, long p_Cur_Page, long p_Page_Size)
        {
            // Initialization.  
            List<M_OTP_DTO> lst;

            SqlParameter p_keyword = new SqlParameter("@p_keyword", (p_Keyword == null) ? "" : p_Keyword);
            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.M_OTP_DTO.FromSqlRaw("p_M_OTP$SEARCH @p_keyword, @p_cur_page, @p_page_size", p_keyword, p_cur_page, p_page_size).ToList<M_OTP_DTO>();
            return lst;
        }
    }
}
