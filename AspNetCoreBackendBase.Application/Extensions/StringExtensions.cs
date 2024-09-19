namespace AspNetCoreBackendBase.Application.Extensions
{
    /// <summary>
    /// Provides helper methods for manipulating strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the "Controller" suffix from the string if it exists.
        /// </summary>
        /// <param name="value">The <see langword="string"/> from which to remove the suffix.</param>
        /// <returns>
        /// The modified <see langword="string"/> without the "Controller" suffix if it was present; otherwise, returns the original <see langword="string"/>.
        /// </returns>
        public static string RemoveControllerSuffix(this string value)
        {
            const string suffix = "Controller";

            // Check if the string ends with the suffix and remove it if present
            return value.EndsWith(suffix) ? value[..^suffix.Length] : value;
        }
    }
}
