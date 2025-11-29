namespace ConectOne.Domain.Constants
{
    /// <summary>
    /// Provides constant values for keys used in local and server storage operations.
    /// </summary>
    /// <remarks>This class contains nested classes that group related storage key constants for use when
    /// storing or retrieving data from local or server-side storage. These constants help ensure consistency and reduce
    /// the risk of typographical errors when accessing storage keys throughout the application.</remarks>
    public static class StorageConstants
    {
        /// <summary>
        /// Provides constant string keys for accessing local client storage values such as user preferences,
        /// authentication tokens, and user information.
        /// </summary>
        /// <remarks>This class contains only static members and is not intended to be instantiated. The
        /// constants can be used as keys when storing or retrieving values from local storage mechanisms, such as
        /// browser local storage or application settings.</remarks>
        public static class Local
        {
            public static string Preference = "clientPreference";

            public static string AuthToken = "authToken";
            public static string RefreshToken = "refreshToken";
            public static string UserImageURL = "userImageURL";
            public static string Company = "NeuralTechCompany";
        }

        /// <summary>
        /// Provides server-related constants and configuration values.
        /// </summary>
        public static class Server
        {
            public static string Preference = "serverPreference";
        }
    }
}
