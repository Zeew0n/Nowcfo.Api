using Newtonsoft.Json;

namespace Nowcfo.Application.Dtos.Email
{
    public class SendGridErrorResponse
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("help")]
        public string Help { get; set; }
    }
}