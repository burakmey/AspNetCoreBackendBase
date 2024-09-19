using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.Helpers
{
    /// <summary>
    /// Provides helper methods for encoding and decoding strings using Base64 URL encoding.
    /// </summary>
    public static class Base64UrlCodeHelper
    {
        /// <summary>
        /// Gets the Base64 URL-encoded representation of the specified string.
        /// </summary>
        /// <param name="value">The <see langword="string"/> to encode.</param>
        /// <returns>
        /// A <see langword="string"/> representing the Base64 URL-encoded value of the input string.
        /// </returns>
        public static string UrlEncode(this string value)
        {
            // Convert the string into a byte array using UTF-8 encoding.
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            // Encode the byte array into a Base64 URL-encoded string.
            return WebEncoders.Base64UrlEncode(bytes);
        }

        /// <summary>
        /// Gets the original string by decoding the specified Base64 URL-encoded string.
        /// </summary>
        /// <param name="value">The Base64 URL-encoded <see langword="string"/> to decode.</param>
        /// <returns>
        /// A <see langword="string"/> representing the original value decoded from the Base64 URL-encoded input.
        /// </returns>
        public static string UrlDecode(this string value)
        {
            // Decode the Base64 URL-encoded string into a byte array.
            byte[] bytes = WebEncoders.Base64UrlDecode(value);

            // Convert the byte array back into a string using UTF-8 encoding.
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
