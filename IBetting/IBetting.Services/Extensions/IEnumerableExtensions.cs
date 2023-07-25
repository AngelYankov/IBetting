using System.Data;
using System.Reflection;

namespace IBetting.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dataTable = new DataTable("DataTable");
            Type t = typeof(T);
            PropertyInfo[] propertyInfos = t.GetProperties();

            //Inspect the properties and create the columns in the DataTable
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type ColumnType = propertyInfo.PropertyType;
                if ((ColumnType.IsGenericType))
                {
                    ColumnType = ColumnType.GetGenericArguments()[0];
                }
                dataTable.Columns.Add(propertyInfo.Name, ColumnType);
            }

            //Populate the data table
            foreach (var item in collection)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow.BeginEdit();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
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
