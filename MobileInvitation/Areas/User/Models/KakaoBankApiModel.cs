using System.Text.Json.Serialization;

namespace MobileInvitation.Areas.User.Models
{
	#region Kakao Firm 모델
	public class KP_Firm
	{
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("api_key")]
		public string ApiKey { set; get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("org_code")]
		public string OrgCode { set; get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("telegram_no")]
		public int? TelegramNo { set; get; }
	}
	public class KP_FirmAccount : KP_Firm
    {
		[JsonPropertyName("number")]
		public string Number { set; get; }
		[JsonPropertyName("bank_code")]
		public string BankCode { set; get; }
	}
	public class KP_FirmInquireDepositor : KP_Firm
	{
		[JsonPropertyName("bank_code")]
		public string BankCode { set; get; }
		[JsonPropertyName("account")]
		public string Account { set; get; }
	}
	public class KP_FirmInquireDepositorEx : KP_FirmInquireDepositor
	{
		public string Depositor { set; get; }
	}

	public class KP_FirmTransfer : KP_Firm
	{
		[JsonPropertyName("drw_account_cntn")]
		public string DrwAccountCntn { set; get; }
		[JsonPropertyName("drw_bank_code")]
		public string DrwBankCode { set; get; }
		[JsonPropertyName("drw_account")]
		public string DrwAccount { set; get; }
		[JsonPropertyName("rv_bank_code")]
		public string RvBankCode { set; get; }
		[JsonPropertyName("rv_account")]
		public string RvAccount { set; get; }
		[JsonPropertyName("rv_account_cntn")]
		public string RvAccountCntn { set; get; }
		[JsonPropertyName("amount")]
		public int Amount { set; get; }
	}
	#endregion

	#region Kakao Result 모델
	public class KP_Result
	{
		[JsonPropertyName("status")]
		public int Status { get; set; }
		[JsonPropertyName("error_code")]
		public string ErrorCode { set; get; }
		[JsonPropertyName("error_message")]
		public string ErrorMessage { set; get; }
	}
	public class KP_FirmInquireDepositorResult : KP_Result
	{
		[JsonPropertyName("natv_tr_no")]
		public string NatvTrNo { set; get; }
		[JsonPropertyName("request_at")]
		public string RequestAt { set; get; }
		[JsonPropertyName("depositor")]
		public string Depositor { set; get; }
	}
	public class KP_FirmTransferResult : KP_Result
	{
		[JsonPropertyName("natv_tr_no")]
		public string NatvTrNo { set; get; }
		[JsonPropertyName("request_at")]
		public string RequestAt { set; get; }
		[JsonPropertyName("amount")]
		public int Amount { set; get; }
	}

    public class KP_TransferReadyResult : KP_Result
    {
        [JsonPropertyName("tid")]
        public string Tid { get; set; }
        [JsonPropertyName("next_redirect_app_url")]
        public string NextRedirectAppUrl { set; get; }
        [JsonPropertyName("next_redirect_mobile_url")]
        public string NextRedirectMobileUrl { set; get; }
        [JsonPropertyName("next_redirect_pc_url")]
        public string NextRedirectPcUrl { set; get; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { set; get; }
    }
    public class KP_TransferApproveResult : KP_Result
    {
        public KP_TransferApproveResult()
        {
            Account = new KP_FirmAccount();
        }
        [JsonPropertyName("cid")]
        public string Cid { set; get; }
        [JsonPropertyName("tid")]
        public string Tid { set; get; }
        [JsonPropertyName("partner_order_id")]
        public string PartnerOrderId { set; get; }
        [JsonPropertyName("partner_user_id")]
        public string PartnerUserId { set; get; }
        [JsonPropertyName("sender_name")]
        public string SenderName { set; get; }
        [JsonPropertyName("total_amount")]
        public int? TotalAmount { set; get; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { set; get; }
        [JsonPropertyName("account")]
        public KP_FirmAccount Account { set; get; }
    }
    #endregion

    #region Kakao Status 모델
    class KP_Status
	{
		[JsonPropertyName("api_key")]
		public string ApiKey { set; get; }
		[JsonPropertyName("cid")]
		public string Cid { set; get; }
		[JsonPropertyName("tid")]
		public string Tid { set; get; }
	}

    public class KP_TransferCallback
    {
        [JsonPropertyName("send_status")]
        public string send_status { set; get; }
        [JsonPropertyName("cid")]
        public string cid { set; get; }
        [JsonPropertyName("tid")]
        public string tid { set; get; }
        [JsonPropertyName("partner_order_id")]
        public string partner_order_id { set; get; }
        [JsonPropertyName("partner_user_id")]
        public string partner_user_id { set; get; }
        [JsonPropertyName("sender_name")]
        public string sender_name { set; get; }
        [JsonPropertyName("total_amount")]
        public int total_amount { set; get; }
        [JsonPropertyName("created_at")]
        public string created_at { set; get; }
        [JsonPropertyName("approved_at")]
        public string approved_at { set; get; }
        [JsonPropertyName("account")]
        public KP_FirmAccount account { set; get; } = new KP_FirmAccount();
    }
    public class KP_StatusResult : KP_TransferCallback
    {
		[JsonPropertyName("status")]
		public int status { set; get; }
	}
    #endregion

    #region KP Transfer 모델
    public class KP_Transfer
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { set; get; }
        [JsonPropertyName("cid")]
        public string Cid { set; get; }
    }
    public class KP_TransferReady : KP_Transfer
    {
        public KP_TransferReady()
        {
            Account = new KP_FirmAccount();
        }
        [JsonPropertyName("partner_order_id")]
        public string PartnerOrderId { set; get; }
        [JsonPropertyName("partner_user_id")]
        public string PartnerUserId { set; get; }
        [JsonPropertyName("item_name")]
        public string ItemName { set; get; }
        [JsonPropertyName("total_amount")]
        public int? TotalAmount { set; get; }
        [JsonPropertyName("account")]
        public KP_FirmAccount Account { set; get; }
        [JsonPropertyName("sender_name")]
        public string SenderName { set; get; }
        [JsonPropertyName("approval_url")]
        public string ApprovalUrl { set; get; }
        [JsonPropertyName("cancel_url")]
        public string CancelUrl { set; get; }
        [JsonPropertyName("fail_url")]
        public string FailUrl { set; get; }
        [JsonPropertyName("callback_url")]
        public string CallbackUrl { set; get; }
    }
    public class KP_TransferApprove : KP_Transfer
    {
        public KP_TransferApprove()
        {
            Account = new KP_FirmAccount();
        }
        [JsonPropertyName("tid")]
        public string Tid { set; get; }
        [JsonPropertyName("partner_order_id")]
        public string PartnerOrderId { set; get; }
        [JsonPropertyName("partner_user_id")]
        public string PartnerUserId { set; get; }
        [JsonPropertyName("pg_token")]
        public string PgToken { set; get; }
        [JsonPropertyName("account")]
        public KP_FirmAccount Account { set; get; }
    }
    public class KP_TransferStatus : KP_Transfer
    {
        [JsonPropertyName("tid")]
        public string Tid { set; get; }
    }
    #endregion
}
