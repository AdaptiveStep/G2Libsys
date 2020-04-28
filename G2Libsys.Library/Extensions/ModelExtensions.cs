using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace G2Libsys.Library.Extensions
{
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
    }
}
