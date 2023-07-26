using System.Data;
using System.Reflection;

namespace IBetting.DataAccess.Extensions
{
    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dataTable = new DataTable("DataTable");
            Type t = typeof(T);
            PropertyInfo[] propertyInfos = t.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type propertyType = propertyInfo.PropertyType;

                if (propertyType.IsClass && propertyType != typeof(string))
                {
                    continue;
                }

                if ((propertyType.IsGenericType))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }
                dataTable.Columns.Add(propertyInfo.Name, propertyType);
            }

            foreach (var item in collection)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.BeginEdit();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Type propertyType = propertyInfo.PropertyType;

                    if (propertyType.IsClass && propertyType != typeof(string))
                    {
                        continue;
                    }

                    if (propertyInfo.GetValue(item, null) != null)
                    {
                        dataRow[propertyInfo.Name] = propertyInfo.GetValue(item, null);
                    }
                }
                dataRow.EndEdit();
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
