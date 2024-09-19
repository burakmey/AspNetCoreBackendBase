using Serilog;

namespace AspNetCoreBackendBase.Application.Loggers
{
    /// <summary>
    /// Provides functionality for initializing and retrieving a <see cref="Log"/> logger instance.
    /// </summary>
    public static class SeriLogger
    {
        static ILogger? _logger;

        /// <summary>
        /// Gets the singleton Serilog <see cref="ILogger"/> instance, initializing it if necessary.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> instance used for logging throughout the application.
        /// </value>
        public static ILogger GetSeriLogger => GetLogger();

        /// <summary>
        /// Retrieves the <see cref="ILogger"/> instance, initializing it if necessary.
        /// <para>
        /// This method configures the logger with a minimum level of debug and outputs the log to the console.
        /// If the logger is already initialized, it returns the existing instance.
        /// </para>
        /// </summary>
        /// <returns>
        /// The initialized or existing <see cref="ILogger"/> instance.
        /// </returns>
        static ILogger GetLogger()
        {
            if (_logger != null)
            {
                //_logger.Debug("Seri log already exists");
                return _logger;
            }

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()  // Set the minimum log level
                .WriteTo.Console()     // Log to the console
                .CreateLogger();

            return _logger;
            // Optionally, you can add more sinks, enrichers, or configurations here
        }
    }
}
