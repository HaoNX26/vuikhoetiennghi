using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.Master
{
    [Table("S_USER")]
    public class User
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("ASC_ID")]
        public long AscId { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("ROLES")]
        public string Roles { get; set; }
        [Column("LASTEST_LOGIN_DATE")]
        public DateTime? LastestLoginDate { get; set; }
        [Column("NUM_OF_LOGIN_FAILT")]
        public long NumLoginFailt { get; set; }
        [Column("IS_EXPIRED")]
        public bool IsExpired { get; set; }
        [Column("CREATED_BY")]
        public int? CreatedBy { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("MODIFIED_BY")]
        public int? ModifiedBy { get; set; }
        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }
    }
}
