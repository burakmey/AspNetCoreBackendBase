using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace AspNetCoreBackendBase.API.Serilog
{
    public class UserNameColumnWriter : ColumnWriterBase
    {
        private readonly string _propertyName;

        // Constructor to initialize the column writer with a specified property name.
        public UserNameColumnWriter(string propertyName) : base(NpgsqlDbType.Varchar)
        {
            _propertyName = propertyName;
        }

        // Retrieves the value of the specified property from the log event.
        public override object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
        {
            // Attempts to get the property from the log event's properties.
            var (username, value) = logEvent.Properties.FirstOrDefault(p => p.Key == _propertyName);
            // Returns the value as a string or null if the property is not found.
            return value?.ToString() ?? null;
        }
    }
}
