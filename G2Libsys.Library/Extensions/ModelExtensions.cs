namespace G2Libsys.Library.Extensions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class ModelExtensions
    {
        /// <summary>
        /// Take model name "Model" and return "models" to match database table
        /// </summary>
        /// <returns></returns>
        public static string ToTableName(this Type model)
        {
            string result = model.Name.ToLower();

            if (result.Last().Equals("s"))
                return result;
            else
            {
                return result + "s";
            }
        }

        /// <summary>
        /// Validate basic email pattern
        /// </summary>
        /// <param name="email"></param>
        public static bool IsValidEmail(this string email)
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
