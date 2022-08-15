using JoinReward.Models.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using JoinReward.Models;
using JoinReward.Models.SubmitCustomerWin;
using JoinReward.Models.Log;
using JoinReward.Models.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace JoinReward.Models.DB
{
    public class BusinessContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private HttpContext _context => _httpContextAccessor.HttpContext;

        public BusinessContext(DbContextOptions<BusinessContext> options)
        : base(options)
        {
        }

        public DbSet<ASCMaster> aSCMasters { get; set; }

        public DbSet<FileCustomerWinSearchDTO> fileCustomerWinSearchDTOs { get; set; }
        public DbSet<ListWinnerSearchDTO> listWinnerSearchDTOs { get; set; }

        public DbSet<SubmitInfoCustomerWin> submitInfoCustomerWins { get; set; }
        public DbSet<BCustomerWin> bCustomerWins { get; set; }
        public DbSet<NoteCustomer> noteCustomers { get; set; }

        public DbSet<V_B_SUBMIT_CUSTOMER_WIN> V_B_SUBMIT_CUSTOMER_WIN { get; set; }
        public DbSet<B_INPUT_PHONE_LOG> B_INPUT_PHONE_LOG { get; set; }

        public List<FileCustomerWinSearchDTO> SearchFileCustomerWin(FileCustomerWinDTO fileCustomerWinDTO, long p_Cur_Page, long p_Page_Size)

        {
            // Initialization.  
            List<FileCustomerWinSearchDTO> lst;
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
            lst = this.fileCustomerWinSearchDTOs.FromSqlRaw("p_file_cusomer_win$Search @CUSTOMER_NAME, " +
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
                p_cur_page, p_page_size).ToList<FileCustomerWinSearchDTO>();
            return lst;
        }

        public List<ListWinnerSearchDTO> SearchWinner(FileCustomerWinDTO fileCustomerWinDTO, long p_Cur_Page, long p_Page_Size)

        {
            // Initialization.  
            List<ListWinnerSearchDTO> lst;
            CultureInfo provider = CultureInfo.InvariantCulture;
            SqlParameter p_CustomerName = new SqlParameter("@CUSTOMER_NAME", (fileCustomerWinDTO.CustmerName == null) ? "" : fileCustomerWinDTO.CustmerName);
            SqlParameter p_PhoneNumber = new SqlParameter("@PHONE_NUMBER", (fileCustomerWinDTO.PhoneNumber == null) ? "" : fileCustomerWinDTO.PhoneNumber);
            SqlParameter p_LuckyCode = new SqlParameter("@LUCKY_CODE", (fileCustomerWinDTO.LuckyCode == null) ? "" : fileCustomerWinDTO.LuckyCode);
            SqlParameter p_ROUND_ID = new SqlParameter("@ROUND_ID", fileCustomerWinDTO.ROUND_ID);


            SqlParameter p_cur_page = new SqlParameter("@p_cur_page", p_Cur_Page);
            SqlParameter p_page_size = new SqlParameter("@p_page_size", p_Page_Size);
            // Processing.  
            lst = this.listWinnerSearchDTOs.FromSqlRaw("p_list_winner$search @CUSTOMER_NAME, " +
                "@PHONE_NUMBER, " +
                "@LUCKY_CODE, " +
                "@ROUND_ID," +
                "@p_cur_page, @p_page_size",
                p_CustomerName,
                p_PhoneNumber,
                p_LuckyCode,
                p_ROUND_ID,
                p_cur_page, p_page_size).ToList<ListWinnerSearchDTO>();
            return lst;
        }
    }
}
