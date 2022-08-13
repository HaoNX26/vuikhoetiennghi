using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinReward.Models.SubmitCustomerWin
{
    [Table("B_NOTE_CUSTOMER")]
    public class NoteCustomer
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("SUBMIT_CUSTOMER_WIN_ID")]
        public long SubmitCustomerWinId { get; set; }
        [Column("NOTE_OF_PANASONIC")]
        public string NoteOfPanasonic { get; set; }
        [Column("NOTE_OF_ASC")]
        public string NoteOfAsc { get; set; }
        [Column("NOTE_OF_AGENCY")]
        public string NoteOfAgency { get; set; }
        [Column("NOTE_OF_CALL_CENTER")]
        public string NoteOfCallCenter { get; set; }
        [Column("FILE_UPLOAD_OF_ASC")]
        public string FileUploadOfASC { get; set; }
        [Column("CREATED_BY")]
        public long? CreatedBy { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("MODIFIED_BY")]
        public long? ModifiedBy { get; set; }
        [Column("MODIFY_DATE")]
        public DateTime? ModifyDate { get; set; }
    }
}
