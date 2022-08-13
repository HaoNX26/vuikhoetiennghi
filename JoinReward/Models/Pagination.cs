using System;

namespace JoinReward.Models
{
    public class Pagination
    {
        public long PAGE_SIZE { get; set; }
        public long CUR_PAGE { get; set; }
        public String CONTROLLER { get; set; }
        public String ACTION { get; set; }
        public long TOTAL_RECORD { get; set; }
        public String GET_QUERY_STRING { get; set; }
    }
}
