
namespace Patheyam.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml.Linq;

    public static class ExtensionMethods
    {
        public static string ToXMLString<T>(this List<T> type, string rootNode, string parentNode)
        {
            return new XElement(rootNode, type.Select(i => new XElement(parentNode, new object[] { i.GetType().GetProperties().Select(x => new XElement(x.Name, x.GetValue(i, null))) }))).ToString();
        }

        public static void ThrowIfNullOrEmpty(this string argumentValue, string message, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(argumentValue)) throw new ValidationException(message, parameterName);
        }

        public static void ThrowIfNullOrEmpty<T>(this List<T> list, string message, string parameterName)
        {
            if (list == null || !list.Any())
            {
                throw new ValidationException(message, parameterName);
            }
        }

        public static void ThrowIfNull(this object argumentValue, string message, string parameterName)
        {
            if (argumentValue == null) throw new ValidationException(message, parameterName);
        }

        public static int ThrowIfNotPositiveInt(this int argumentValue, string message, string parameterName)
        {
            if (argumentValue < 0) throw new ValidationException(message, parameterName);

            return argumentValue;
        }

        public static int ThrowIfNotPositiveNonZeroInt(this int argumentValue, string message, string parameterName)
        {
            if (argumentValue <= 0) throw new ValidationException(message, parameterName);

            return argumentValue;
        }

        public static decimal ThrowIfNotPositiveDecimal(this string argumentValue, string message, string parameterName)
        {
            var success = decimal.TryParse(argumentValue, out decimal result);
            if (!success || result < 0) throw new ValidationException(message, parameterName);

            return result;
        }

        public static decimal ThrowIfNotPositiveNonZeroDecimal(this string argumentValue, string message, string parameterName)
        {
            var success = decimal.TryParse(argumentValue, out decimal result);
            if (!success || result <= 0) throw new ValidationException(message, parameterName);

            return result;
        }

        public static DataTable ConvertToDataTable<T>(this List<T> list, string columnName)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(columnName, typeof(T));
            list.ForEach(item => dataTable.Rows.Add(item));
            return dataTable;
        }
    }
}
