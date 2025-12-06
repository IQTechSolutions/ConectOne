using System.Text.Json;
using Recapcha.Entities;

namespace Recapcha.Services
{
    /// <summary>
    /// Provides functionality to verify Google reCAPTCHA v3 tokens.
    /// </summary>
    /// <remarks>This service sends a verification request to the Google reCAPTCHA v3 API to validate the
    /// provided token. Ensure that the secret key used in the request is configured correctly and kept
    /// secure.</remarks>
    public class GooglereCAPTCHAv3Service
    {
        /// <summary>
        /// Verifies the provided reCAPTCHA v3 token by sending it to the Google reCAPTCHA verification API.
        /// </summary>
        /// <remarks>This method sends an HTTP POST request to the Google reCAPTCHA verification endpoint
        /// with the provided token and a secret key. Ensure that the token is valid and has not expired before calling
        /// this method.</remarks>
        /// <param name="token">The reCAPTCHA v3 token to be verified. This token is typically obtained from the client-side reCAPTCHA
        /// integration.</param>
        /// <returns>A <see cref="GooglereCAPTCHAv3Response"/> object containing the verification result, or <see
        /// langword="null"/> if the response could not be deserialized.</returns>
        public virtual async Task<GooglereCAPTCHAv3Response?> Verify(string token)
        {
            GooglereCAPTCHAv3Response? reCaptchaResponse;
            using (var httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("secret", "6LfI77wrAAAAAKlzzj4VmzWzUTfMXKbwztK9MBdK"),
                    new KeyValuePair<string, string>("response", token)
                });
                try
                {
                    var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    reCaptchaResponse = JsonSerializer.Deserialize<GooglereCAPTCHAv3Response>(jsonString);
                }
                catch (Exception ex)
                {
                    throw;
                }

                return reCaptchaResponse;
            }
        }
    }
}
