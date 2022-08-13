using JoinReward.Models.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using JoinReward.Models;
using JoinReward.Models.SubmitCustomerWin;
using JoinReward.Models.Log;

namespace JoinReward.Models.DB
{
    public class SubmitCustomerWinContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private HttpContext _context => _httpContextAccessor.HttpContext;

        public SubmitCustomerWinContext(DbContextOptions<SubmitCustomerWinContext> options)
        : base(options)
        {
        }

        public DbSet<ASCMaster> aSCMasters { get; set; }

        public DbSet<SubmitInfoCustomerWin> submitInfoCustomerWins { get; set; }
        public DbSet<BCustomerWin> bCustomerWins { get; set; }
        public DbSet<NoteCustomer> noteCustomers { get; set; }

        public DbSet<V_B_SUBMIT_CUSTOMER_WIN> V_B_SUBMIT_CUSTOMER_WIN { get; set; }
        public DbSet<B_INPUT_PHONE_LOG> B_INPUT_PHONE_LOG { get; set; }
    }
}
