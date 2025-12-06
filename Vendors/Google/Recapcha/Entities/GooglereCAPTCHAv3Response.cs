using Newtonsoft.Json;

namespace Recapcha.Entities
{
    /// <summary>
    /// Represents the response from a Google reCAPTCHA v3 verification request.
    /// </summary>
    /// <remarks>This class encapsulates the data returned by the Google reCAPTCHA v3 API, including the
    /// success status,  score, action, timestamp of the challenge, and the hostname of the request origin. It is
    /// typically used  to evaluate whether a user interaction is legitimate or potentially automated.</remarks>
    public class GooglereCAPTCHAv3Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        [JsonProperty("success")] public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the score value associated with the object.
        /// </summary>
        [JsonProperty("score")] public double Score { get; set; }

        /// <summary>
        /// Gets or sets the action to be performed.
        /// </summary>
        [JsonProperty("action")] public string Action { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the challenge request.
        /// </summary>
        [JsonProperty("challenge_ts")] public DateTime Challenge_ts { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the server or endpoint.
        /// </summary>
        [JsonProperty("hostname")] public string Hostname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("errorcodes")] public string[] ErrorCodes { get; set; }
    }
}
