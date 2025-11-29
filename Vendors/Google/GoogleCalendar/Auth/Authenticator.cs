using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GoogleCalendar.Auth
{
    /// <summary>
    /// The <see cref="Authenticator"/> class manages the Google OAuth 2.0 
    /// flow for a service account, providing methods to:
    /// 1. Authenticate using a JSON-embedded service account credential.
    /// 2. Load and store tokens (optional for certain use cases).
    /// </summary>
    public class Authenticator
    {
        // Defines the path/filename to which tokens can be saved or loaded.
        private static readonly string TokenFile = "stored-token.json";

        /// <summary>
        /// Authenticates with Google APIs using the given scope. 
        /// Returns an <see cref="IConfigurableHttpClientInitializer"/> which can be used 
        /// to create a new <see cref="Google.Apis.Services.BaseClientService"/>.
        /// </summary>
        /// <param name="scope">A single scope required by the Google API (e.g., calendar read/write).</param>
        /// <returns>An OAuth 2.0 credential object with the requested scopes.</returns>
        public static IConfigurableHttpClientInitializer Authenticate(string scope)
        {
            // Build a list of scopes (the provided one plus a read-only calendar scope).
            var scopes = new[] { scope, "https://www.googleapis.com/auth/calendar.readonly" };

            try
            {
                // Locate the service account JSON file. The file name may contain
                // a unique identifier, so search for the first match.
                var credentialDir = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "googlecalendar");

                var credentialPath = Directory
                    .GetFiles(credentialDir, "eversdal-calendar-*.json")
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(credentialPath))
                {
                    throw new FileNotFoundException(
                        "Service account credentials not found in '" + credentialDir + "'.");
                }

                using FileStream stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read);
                GoogleCredential credential = GoogleCredential.FromStream(stream);

                // Create a credential scoped to the required permissions.
                return credential.CreateScoped(scopes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Loads a previously saved <see cref="TokenResponse"/> from disk (if present). 
        /// This is primarily for user credentials rather than service accounts, but 
        /// can be repurposed if the application stores tokens in a file for offline reuse.
        /// </summary>
        /// <param name="scope">The scope relevant to this token.</param>
        /// <returns>The <see cref="TokenResponse"/> loaded from file, or null if not found.</returns>
        public static TokenResponse LoadToken(string scope)
        {
            var tokenFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "googlecalendar", TokenFile);
            if (!File.Exists(tokenFile))
            {
                return null;
            }

            using (StreamReader sr = File.OpenText(tokenFile))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);

                // Optionally store/update the scope in the JSON object.
                json["scope"] = scope;

                return (TokenResponse)json.ToObject(typeof(TokenResponse));
            }
        }

        /// <summary>
        /// Saves a <see cref="TokenResponse"/> to disk as JSON. 
        /// This allows tokens to be reused across application runs.
        /// </summary>
        /// <param name="token">The token to serialize and store.</param>
        public static void StoreToken(TokenResponse token)
        {
            var tokenFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "googlecalendar", TokenFile);
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(tokenFile))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = ' ';
                writer.Indentation = 2;

                serializer.Serialize(writer, token);
            }
        }
    }
}
