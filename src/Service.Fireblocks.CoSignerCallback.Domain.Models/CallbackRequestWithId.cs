namespace Service.Fireblocks.CoSignerCallback.Domain.Models
{
    public class CallbackRequestWithId
    {
        [Newtonsoft.Json.JsonProperty("requestId")]
        public string RequestId { get; set; }
    }
}
