namespace Service.Fireblocks.CoSignerCallback.Models
{
    public class CallbackResponse
    {
        [Newtonsoft.Json.JsonProperty("action")]
        public string Action { get; set; }

        [Newtonsoft.Json.JsonProperty("requestId")]
        public string RequestId { get; set; }

        [Newtonsoft.Json.JsonProperty("rejectionReason")]
        public string RejectionReason { get; set; }
    }
}
