using Microsoft.Extensions.Configuration;

namespace AspNetCoreBackendBase.Application
{
    /// <summary>
    /// Provides methods to load and retrieve configuration settings from JSON files.
    /// <para>
    /// This class handles configuration management for the application, 
    /// loading environment-specific settings from <c>appsettings.json</c> and 
    /// <c>appsettings.[Environment].json</c>.
    /// </para>
    /// </summary>
    public static class Configuration
    {
        static IConfigurationRoot? _configuration;

        /// <summary>
        /// Gets the singleton <see cref="IConfigurationRoot"/> instance, initializing it if necessary.
        /// </summary>
        /// <value>
        /// The <see cref="IConfigurationRoot"/> instance used for accessing application configuration settings.
        /// </value>
        public static IConfigurationRoot GetConfigurationRoot => GetConfiguration();

        /// <summary>
        /// Loads the configuration settings from JSON files based on the current environment.
        /// <para>
        /// This method first checks the environment variable <c>ASPNETCORE_ENVIRONMENT</c> to 
        /// determine whether the application is running in "Development" or another environment.
        /// It loads the common <c>appsettings.json</c> and environment-specific JSON file, 
        /// e.g., <c>appsettings.Development.json</c>.
        /// </para>
        /// </summary>
        /// <returns>
        /// An <see cref="IConfigurationRoot"/> object containing the loaded configuration settings.
        /// </returns>
        static IConfigurationRoot GetConfiguration()
        {
            // Check if the configuration has already been loaded.
            if (_configuration != null)
                return _configuration;

            // Determines the current environment from the environment variable 'ASPNETCORE_ENVIRONMENT'.
            // If the variable is not set, default to "Development".
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = (Directory.GetParent(currentDirectory)?.FullName) ?? throw new DirectoryNotFoundException("Parent directory not found.");

            // Find the subdirectory in the parent directory that contains "API" in its name.
            var apiDirectory = Directory.GetDirectories(parentDirectory)
                .FirstOrDefault(d => Path.GetFileName(d).Contains("API")) ?? throw new DirectoryNotFoundException("No directory containing 'API' found in the parent directory.");

            // Set the base path for the configuration files based on the environment.
            var path = environment == "Development"
                ? apiDirectory
                : currentDirectory;

            // Create a configuration builder and set its base path to the directory determined above.
            // Add two JSON configuration files: the common 'appsettings.json' and the environment-specific 
            // 'appsettings.[Environment].json' (e.g., 'appsettings.Development.json' or 'appsettings.Production.json').
            // The files are marked as optional, and the settings will reload automatically if the files change.
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            // Build the configuration and cache it in the '_configuration' field to avoid reloading it multiple times.
            _configuration = builder.Build();

            // Return the loaded configuration.
            return _configuration;
        }

        /// <summary>
        /// Retrieves a required configuration value by its key.
        /// <para>
        /// If the key is missing or its value is null/empty, an exception will be thrown. 
        /// Use this method when the configuration value is mandatory for application execution.
        /// </para>
        /// </summary>
        /// <param name="key">The key of the configuration setting to retrieve.</param>
        /// <returns>
        /// A <see langword="string"/> representing the value associated with the specified key.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown if the key is not found or its value is null/empty.
        /// </exception>
        static string GetRequiredValue(string key)
        {
            //// Load the configuration settings from the environment and JSON files.
            //var config = GetConfiguration();

            // Retrieve the configuration value associated with the specified key.
            var value = GetConfigurationRoot[key];

            // Check if the value is null or empty.
            // If the value is missing, throw an exception indicating that the key was not found in the configuration.
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"{key} not found in configuration!");
            }

            // Return the configuration value.
            return value;
        }


        // Default Configurations
        public static string GetOrigin => GetRequiredValue("Origin");

        // ConnectionString Configurations
        public static string GetConnectionStringPostgresql => GetRequiredValue("ConnectionString:Postgresql");

        // ExternalLogin Configurations
        public static string GetExternalLoginGoogleClientId => GetRequiredValue("ExternalLogin:GoogleClientId");

        // Mail Configurations
        public static string GetMailDisplayName => GetRequiredValue("Mail:DisplayName");
        public static string GetMailUserName => GetRequiredValue("Mail:UserName");
        public static string GetMailPassword => GetRequiredValue("Mail:Password");
        public static string GetMailPort => GetRequiredValue("Mail:Port");
        public static string GetMailHost => GetRequiredValue("Mail:Host");

        // Storage Configurations
        public static string GetSerilogPropertyUserName => GetRequiredValue("Serilog:Properties:UserName");

        // Storage Configurations
        public static string GetStorageAzure => GetRequiredValue("Storage:Azure");

        // Token Configurations
        public static string GetTokenAccessTokenMinute => GetRequiredValue("Token:AccessTokenMinute");
        public static string GetTokenRefreshTokenMinute => GetRequiredValue("Token:RefreshTokenMinute");
    }
}
