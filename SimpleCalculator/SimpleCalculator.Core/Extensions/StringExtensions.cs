using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for <c>string</c> type.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Throws the <see cref="ArgumentNullException"/>
        /// if <paramref name="source"/> is <c>null</c> or empty.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <param name="parameterName">The parameter name of the string to check.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullOrEmptyException(this string source, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws the <see cref="ArgumentNullException"/>
        /// if <paramref name="source"/> is <c>null</c> or empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <param name="parameterName">The parameter name of the string to check.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullOrWhitespaceException(this string source, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Removes all whitespaces in the target string.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <returns>The string with all whitespaces removed.</returns>
        [return: NotNull]
        public static string RemoveAllWhitespaces(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            return Regex.Replace(target, "\\s", string.Empty);
        }

        /// <summary>
        /// Splits the given string with whitespace.
        /// </summary>
        /// <param name="source">The string to split.</param>
        /// <returns>A collection of the split string with whitespace.</returns>
        [return: NotNull]
        public static IEnumerable<string> SplitWithWhitespace(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Enumerable.Empty<string>();
            }

            // Extracts non-whitespace characters
            return Regex.Matches(source, "\\S+").OfType<Match>().Select(m => m.Value);
        }
    }
}
