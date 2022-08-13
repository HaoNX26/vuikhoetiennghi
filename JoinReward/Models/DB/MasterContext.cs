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
        public DbSet<AssignAscSearchDTO> assignAscSearchDTOs { get; set; }
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
        public List<AssignAscSearchDTO> SearchSubmitCustomerWin(FileCustomerWinDTO fileCustomerWinDTO, long p_Cur_Page, long p_Page_Size)
        {
            // Initialization.  
            List<AssignAscSearchDTO> lst;

            SqlParameter p_CustomerName = new SqlParameter("@CUSTOMER_NAME", (fileCustomerWinDTO.CustmerName == null) ? "" : fileCustomerWinDTO.CustmerName);
            SqlParameter p_PhoneNumber = new SqlParameter("@PHONE_NUMBER", (fileCustomerWinDTO.PhoneNumber == null) ? "" : fileCustomerWinDTO.PhoneNumber);
            SqlParameter p_LuckyCode = new SqlParameter("@LUCKY_CODE", (fileCustomerWinDTO.LuckyCode == null) ? "" : fileCustomerWinDTO.LuckyCode);
            SqlParameter p_AscId = new SqlParameter("@ASC_ID", fileCustomerWinDTO.AscId);
            SqlParameter p_status_id = new SqlParameter("@STATUS_ID", fileCustomerWinDTO.StatusId);
            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.assignAscSearchDTOs.FromSqlRaw("p_Asign_ASC$Search @CUSTOMER_NAME, @PHONE_NUMBER, @LUCKY_CODE, @ASC_ID, @STATUS_ID, @p_cur_page, @p_page_size", p_CustomerName, p_PhoneNumber, p_LuckyCode, p_AscId, p_status_id, p_cur_page, p_page_size).ToList<AssignAscSearchDTO>();
            return lst;
        }
        public List<AssignAscSearchDTO> SearchAssignAsc(FileCustomerWinDTO     fileCustomerWinDTO, long p_Cur_Page, long p_Page_Size)

        {
            // Initialization.  
            List<AssignAscSearchDTO> lst;
            CultureInfo provider = CultureInfo.InvariantCulture;
            SqlParameter p_CustomerName = new SqlParameter("@CUSTOMER_NAME", (fileCustomerWinDTO.CustmerName == null) ? "" : fileCustomerWinDTO.CustmerName);
            SqlParameter p_PhoneNumber = new SqlParameter("@PHONE_NUMBER", (fileCustomerWinDTO.PhoneNumber == null) ? "" : fileCustomerWinDTO.PhoneNumber);
            SqlParameter p_LuckyCode = new SqlParameter("@LUCKY_CODE", (fileCustomerWinDTO.LuckyCode == null) ? "" : fileCustomerWinDTO.LuckyCode);
            SqlParameter p_FROM_DATE = new SqlParameter("@FROM_DATE", (fileCustomerWinDTO.FROM_DATE == null) ? "" : DateTime.ParseExact(fileCustomerWinDTO.FROM_DATE, "dd/MM/yyyy", provider).ToString("yyyy-MM-dd"));
            SqlParameter p_TO_DATE = new SqlParameter("@TO_DATE", (fileCustomerWinDTO.TO_DATE == null) ? "" : DateTime.ParseExact(fileCustomerWinDTO.TO_DATE, "dd/MM/yyyy", provider).ToString("yyyy-MM-dd"));
            SqlParameter p_AscId = new SqlParameter("@ASC_ID", fileCustomerWinDTO.AscId);
            SqlParameter p_CUSTOMER_STATUS = new SqlParameter("@CUSTOMER_STATUS", fileCustomerWinDTO.CUSTOMER_STATUS);
            SqlParameter p_PAN_STATUS = new SqlParameter("@PAN_STATUS", fileCustomerWinDTO.PAN_STATUS);
            SqlParameter p_ASC_STATUS = new SqlParameter("@ASC_STATUS", fileCustomerWinDTO.ASC_STATUS);

            SqlParameter p_PROVINCE_ID = new SqlParameter("@PROVINCE_ID", fileCustomerWinDTO.PROVINCE_ID);
            SqlParameter p_ROUND_ID = new SqlParameter("@ROUND_ID", fileCustomerWinDTO.ROUND_ID);
            SqlParameter p_PRIZE_WIN = new SqlParameter("@PRIZE_WIN", fileCustomerWinDTO.PRIZE_WIN);
            SqlParameter p_COUNT_SUBMIT = new SqlParameter("@COUNT_SUBMIT", fileCustomerWinDTO.COUNT_SUBMIT);
            SqlParameter p_AGENCY_ID = new SqlParameter("@AGENCY_ID", fileCustomerWinDTO.AGENCY_ID);

            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.assignAscSearchDTOs.FromSqlRaw("p_Asign_ASC$Search @CUSTOMER_NAME, " +
                "@PHONE_NUMBER, " +
                "@LUCKY_CODE, " +
                "@ASC_ID, " +
                "@CUSTOMER_STATUS, " +
                "@PAN_STATUS, " +
                "@ASC_STATUS, " +
                "@PROVINCE_ID," +
                "@ROUND_ID," +
                "@PRIZE_WIN," +
                "@COUNT_SUBMIT," +
                "@AGENCY_ID, " +
                "@FROM_DATE," +
                "@TO_DATE," +
                "@p_cur_page, @p_page_size",
                p_CustomerName, 
                p_PhoneNumber, 
                p_LuckyCode, 
                p_AscId,
                p_CUSTOMER_STATUS,
                p_PAN_STATUS,
                p_ASC_STATUS,
                p_PROVINCE_ID,
                p_ROUND_ID,
                p_PRIZE_WIN,
                p_COUNT_SUBMIT,
                p_AGENCY_ID,
                p_FROM_DATE,
                p_TO_DATE,
                p_cur_page, p_page_size).ToList<AssignAscSearchDTO>();
            return lst;
        }

        public List<GetInfoCustomerWinDTO> SearchListCustomerForAsc(FileCustomerWinDTO fileCustomerWinDTO, long p_Cur_Page, long p_Page_Size)
        { 
            // Initialization.  
            List<GetInfoCustomerWinDTO> lst;

            SqlParameter p_CustomerName = new SqlParameter("@CUSTOMER_NAME", (fileCustomerWinDTO.CustmerName == null) ? "" : fileCustomerWinDTO.CustmerName);
            SqlParameter p_PhoneNumber = new SqlParameter("@PHONE_NUMBER", (fileCustomerWinDTO.PhoneNumber == null) ? "" : fileCustomerWinDTO.PhoneNumber);
            SqlParameter p_LuckyCode = new SqlParameter("@LUCKY_CODE", (fileCustomerWinDTO.LuckyCode == null) ? "" : fileCustomerWinDTO.LuckyCode);
            SqlParameter p_AscId = new SqlParameter("@ASC_ID", fileCustomerWinDTO.AscId);
            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.getInfoCustomerWinDTOs.FromSqlRaw("p_Asign_ASC$Search @CUSTOMER_NAME, @PHONE_NUMBER, @LUCKY_CODE, @ASC_ID, @p_cur_page, @p_page_size", p_CustomerName, p_PhoneNumber, p_LuckyCode, p_AscId, p_cur_page, p_page_size).ToList<GetInfoCustomerWinDTO>();
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
