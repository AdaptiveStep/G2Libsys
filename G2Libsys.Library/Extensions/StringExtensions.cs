namespace G2Libsys.Library.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Method that limits the length of text to a defined length.
        /// </summary>
        /// <param name="message">The source text.</param>
        /// <param name="maxLength">The maximum limit of the string to return.</param>
        public static string LimitLength(this string message, int maxLength)
        {
            if (message.Length <= maxLength)
            {
                return message;
            }

            return message.Substring(0, maxLength) + "...";
        }

    }
}
