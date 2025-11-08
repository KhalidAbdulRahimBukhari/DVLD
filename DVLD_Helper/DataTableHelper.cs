using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Helpers
{
    public static class DataTableHelper
    {
        public static DataTable ToDataTable<T>(List<T> items, params string[] selectedColumns)
        {
            DataTable dt = new DataTable(typeof(T).Name);

            var selectedProps = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);


            if (selectedColumns != null && selectedColumns.Length > 0)
                selectedProps = selectedProps.Where(p => selectedColumns.Contains(p.Name)).ToArray();

            foreach (var prop in selectedProps)
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in items)
            {
                var values = selectedProps.Select(p => p.GetValue(item, null)).ToArray();
                dt.Rows.Add(values);
            }

            return dt;
        }
    }
}
