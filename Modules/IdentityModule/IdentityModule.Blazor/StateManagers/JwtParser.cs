using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using IdentityModule.Domain.Constants;
using Newtonsoft.Json;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// Provides methods to parse JWT tokens and extract claims.
    /// </summary>
    [JsonSourceGenerationOptions(WriteIndented = true)] public static class JwtParser
    {
        /// <summary>
        /// Parses the claims from a JWT token.
        /// </summary>
        /// <param name="jwtToken">The JWT token to parse.</param>
        /// <returns>A collection of claims extracted from the JWT token.</returns>
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken)
        {
            var claims = new List<Claim>();
            if (jwtToken == null)
            {
                return claims;
            }

            // Extract the payload from the JWT token
            var payload = jwtToken.Split('.')[1];

            // Decode the payload from Base64
            var jsonBytes = ParseBase64WithoutPadding(payload);

            // Deserialize the payload into a dictionary
            var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(System.Text.Encoding.UTF8.GetString(jsonBytes));

            // Extract roles and permissions from the JWT token
            ExtractRolesFromJwt(claims, keyValuePairs);
            ExtractPermissionsFromJwt(claims, keyValuePairs);

            // Add remaining key-value pairs as claims
            claims.AddRange(keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        /// <summary>
        /// Extracts roles from the JWT token and adds them to the claims collection.
        /// </summary>
        /// <param name="claims">The claims collection to add the roles to.</param>
        /// <param name="keyValuePairs">The key-value pairs from the JWT payload.</param>
        private static void ExtractRolesFromJwt(List<Claim> claims, Dictionary<string, object>? keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');

                if (parsedRoles.Length > 1)
                {
                    foreach (var role in parsedRoles)
                    {
                        var cleanedRole = TextHelpers.ExtractIdentifier(role);
                        if(!string.IsNullOrEmpty(cleanedRole))
                            claims.Add(new Claim(ClaimTypes.Role, cleanedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }

        /// <summary>
        /// Extracts permissions from the JWT token and adds them to the claims collection.
        /// </summary>
        /// <param name="claims">The claims collection to add the permissions to.</param>
        /// <param name="keyValuePairs">The key-value pairs from the JWT payload.</param>
        private static void ExtractPermissionsFromJwt(List<Claim> claims, Dictionary<string, object>? keyValuePairs)
        {
            keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
            if (permissions != null)
            {
                if (permissions.ToString().Trim().StartsWith("["))
                {
                    var parsedPermissions = JsonConvert.DeserializeObject<string[]>(permissions.ToString());
                    claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                }
                else
                {
                    claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                }
                keyValuePairs.Remove(ApplicationClaimTypes.Permission);
            }
        }

        /// <summary>
        /// Decodes a Base64 string, adding padding if necessary.
        /// </summary>
        /// <param name="base64">The Base64 string to decode.</param>
        /// <returns>The decoded byte array.</returns>
        public static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }

    public static class TextHelpers
    {
        /// <summary>
        /// Extracts the first identifier found in a loosely formatted token.
        /// <para>
        /// • Works with input such as <c>"\n  \"Parent\""</c>, <c>"  \"Child\"  "</c>,
        ///   or even <c>"  Parent "</c> (quotes optional).  
        /// • If a quoted identifier is present, it returns the content between the first
        ///   pair of double quotes.  
        /// • If no quotes are present, it returns the first run of letters, digits,
        ///   or underscores (a typical C-style identifier).  
        /// • Returns <see cref="string.Empty" /> when the input is <c>null</c>,
        ///   empty, or contains no identifier.
        /// </para>
        /// </summary>
        /// <param name="raw">The raw string to parse.</param>
        /// <returns>The extracted identifier, or an empty string.</returns>
        public static string ExtractIdentifier(string? raw)
        {
            // Guard clause for null / whitespace-only input
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            // Trim leading/trailing whitespace and new-lines
            string trimmed = raw.Trim();

            // CASE 1: The identifier is the entire trimmed string and wrapped in quotes
            if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
                return trimmed.Substring(1, trimmed.Length - 2);

            // CASE 2: Search for the first quoted segment anywhere in the string
            Match quoted = Regex.Match(trimmed, "\"(?<id>[^\"]+)\"");
            if (quoted.Success)
                return quoted.Groups["id"].Value;

            // CASE 3: Fall back to “first identifier-looking token” (letters/digits/_)
            Match bare = Regex.Match(trimmed, @"\b(?<id>[A-Za-z_][A-Za-z0-9_]*)\b");
            return bare.Success ? bare.Groups["id"].Value : string.Empty;
        }
    }
}
