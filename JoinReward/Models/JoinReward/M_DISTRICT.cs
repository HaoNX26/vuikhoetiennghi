using System.ComponentModel.DataAnnotations;

namespace PAN.Warranty.Models.Master
{
    public class M_DISTRICT
    {
        [Key]
        public long ID { get; set; }
        public long PROVINCE_ID { get; set; }
        public string DISTRICT_NAME { get; set; }
    }
}
