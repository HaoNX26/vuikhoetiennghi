namespace JoinReward.Models.DTO
{
    public class ListWinnerSearchDTO
    {
		public long ID { get; set; }
		public long CUS_SUBMIT_ID { get; set; }
		public string LUCKY_CODE { get; set; }
		public string PHONE_NUMBER { get; set; }
		public string CUSTOMER_NAME { get; set; }
		public string ADDRESS { get; set; }
		public long ROUND_ID { get; set; }
		public string PRIZE_WIN_TEXT { get; set; }
		public string ROUND_NAME { get; set; }
		public string CUSTOMER_STATUS { get; set; }
		public int NUM_OF_RECORD { get; set; }
	}
}
