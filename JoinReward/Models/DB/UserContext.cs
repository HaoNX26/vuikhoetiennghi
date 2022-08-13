using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
 
using System.Threading.Tasks;
using SysFrameworks;
using JoinReward.Models.DTO;
using JoinReward.Models.User;

namespace JoinReward.Models.DB
{
    public class UserContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private HttpContext _context => _httpContextAccessor.HttpContext;
        public DbSet<AppUserLogin> AppUserLogin { get; set; }
      
        public DbSet<S_USER> S_USER { get; set; }
      
        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public async Task<S_USER> GetUserByAccountName(string usernameVal)

        {

            return await this.S_USER.Where(p => p.USERNAME == usernameVal).FirstOrDefaultAsync<S_USER>();
        }
        public async Task<AppUserLogin> LoginByUsernamePasswordMethodAsync(string usernameVal, string passwordVal)
        {
            // Initialization.  
            //passwordVal = SysFrameworks.Crypto.EncryptString128Bit(passwordVal);//Mã hóa pass
            //passwordVal = LogicPie.Security._Hash.PasswordHash.Hash(passwordVal);//Mã hóa pass

            AppUserLogin lst = new AppUserLogin();
            S_USER s_USER = await this.S_USER.Where(p => p.USERNAME == usernameVal && !p.IS_EXPIRED).FirstOrDefaultAsync<S_USER>();
            if (s_USER != null)
            {
                if (LogicPie.Security._Hash.PasswordHash.Verify(passwordVal, s_USER.PASSWORD))
                {
                    s_USER.LASTEST_LOGIN_DATE = DateTime.Now;
                    this.S_USER.Update(s_USER);
                    this.SaveChanges();
                    s_USER.CopyProperties(lst);
                }
                else
                {
                    lst.ID = 0;
                    lst.RESPONSE_MESSAGE = "Mật khẩu không chính xác!";
                }
            }
            else
            {
                lst.ID = 0;
                lst.RESPONSE_MESSAGE = "Tài khoản không tồn tài trong hệ thống hoặc bị khóa!";
            }
          
            return lst;
        }
       
       
    }
}
