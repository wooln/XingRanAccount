using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using MCS.Library.Core;

namespace XingRanAccountTest
{
    /// <summary>
    /// DataTable扩展, 
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        ///  转换为DataView 列名使用属性名,列Caption使用DescriptionAttribute
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="items">集合</param>
        /// <param name="propertyNames"> </param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, params string[] propertyNames)
        {
            Type type = typeof(T);
            return ToDataTable((IEnumerable<Object>)items, type, propertyNames);
        }

        /// <summary>
        ///  转换为DataView 列名使用属性名,列Caption使用DescriptionAttribute
        /// </summary>
        /// <param name="singleType">元素类型</param>
        /// <param name="items">集合</param>
        /// <param name="propertyNames"> </param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IEnumerable<Object> items, Type singleType, params string[] propertyNames)
        {
            items.NullCheck("items");
            Type type = singleType;

            DataTable table = GetTableStruct(type, propertyNames);

            items.ForEach(obj =>
            {
                DataRow row = table.NewRow();

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (table.Columns.Contains(prop.Name))
                    {
                        object columnValue = prop.GetValue(obj, null);
                        if (columnValue is DateTime && ((DateTime)columnValue) == DateTime.MinValue)
                        {
                            columnValue = DBNull.Value;
                        }
                        row[prop.Name] = columnValue;
                    }
                }
                table.Rows.Add(row);
            });
            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// 获取表结构, column Name使用属性名, Caption使用Description, 没有Description则使用属性名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyTypeNames">不传则导出全部属性</param>
        /// <returns></returns>
        public static DataTable GetTableStruct(Type type, params string[] propertyTypeNames)
        {
            //propertyTypeNames.NullCheck("propertyTypeNames");
            //propertyTypeNames.Any().FalseThrow("propertyTypeNames至少要有一项");

            bool isAll = propertyTypeNames == null || !propertyTypeNames.Any();

            DataTable table = new DataTable("table");

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (isAll || propertyTypeNames.Exists(p => p == prop.Name))
                {
                    var description = AttributeHelper.GetCustomAttribute<DescriptionAttribute>(prop);
                    string columnNameCaption;
                    if (description != null)
                    {
                        columnNameCaption = description.Description;
                    }
                    else
                    {
                        columnNameCaption = prop.Name;
                    }
                    DataColumn column = new DataColumn(prop.Name, prop.PropertyType);
                    column.Caption = columnNameCaption;
                    table.Columns.Add(column);
                }
            }
            table.AcceptChanges();
            return table;
        }

        public static DataTable SwapColumnNameCaption(DataTable table)
        {
            table.NullCheck("table");
            table.Columns.ForEach<DataColumn>(c =>
            {
                var temp = c.Caption;
                c.Caption = c.ColumnName;
                c.ColumnName = temp;
            });
            return table;
        }

        /// <summary>
        /// 根据实际数据类型字段的Descrition重置列name和Caption (主要用途是把excel中的中文列的表变成实体)
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static void RenameColumnByOriginalType(this DataTable sourceTable, Type dataType)
        {
            PropertyInfo[] properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, PropertyInfo> attrs = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DescriptionAttribute attr = AttributeHelper.GetCustomAttribute<DescriptionAttribute>(propertyInfo);
                if (attr != null)
                {
                    attrs.ContainsKey(attr.Description).TrueThrow<ArgumentNullException>("类型[{0}]的[{1}]重复", dataType.FullName, attr.Description);
                    attrs.Add(attr.Description, propertyInfo);
                }

            }

            foreach (DataColumn column in sourceTable.Columns)
            {
                PropertyInfo propertyInfo;
                if (attrs.TryGetValue(column.Caption, out propertyInfo))
                {
                    column.ColumnName = propertyInfo.Name;
                }
            }

        }

        /// <summary>
        /// 转置表格
        /// </summary>
        /// <param name="sourceTable">源</param>
        /// <param name="distinctColumnNamme">转置后的首列</param>
        /// <returns>转置后表格</returns>
        public static DataTable TransPosition(this DataTable sourceTable, string distinctColumnNamme)
        {
            DataTable table = new DataTable();
            table.Columns.Add(distinctColumnNamme, typeof(string));
            //指定列的行数据变成列
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                table.Columns.Add(sourceTable.Rows[i][distinctColumnNamme].ToString());
            }
            //循环每列变成行
            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                var row = table.NewRow();

                string sourceColumnName = sourceTable.Columns[i].ColumnName;
                if (sourceColumnName == distinctColumnNamme) continue;

                row[distinctColumnNamme] = sourceColumnName;
                //循环每行
                for (int j = 0; j < sourceTable.Rows.Count; j++)
                {
                    row[sourceTable.Rows[j][distinctColumnNamme].ToString()]
                        = sourceTable.Rows[j][i];
                }

                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 生成JSON字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CreateJsonParameters(this DataTable dt)
        {
            /* /****************************************************************************
             * Without goingin to the depth of the functioning
             * of this method, i will try to give an overview
             * As soon as this method gets a DataTable it starts to convert it into JSON String,
             * it takes each row and in each row it grabs the cell name and its data.
             * This kind of JSON is very usefull when developer have to have Column name of the .
             * Values Can be Access on clien in this way. OBJ[0].<ColumnName>
             * NOTE: One negative point. by this method user
             * will not be able to call any cell by its index.
             * *************************************************************************/

            StringBuilder JsonString = new StringBuilder();

            //Exception Handling
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() +
                                              "\":" + "\"" +
                                              dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" +
                                              dt.Columns[j].ColumnName.ToString() + "\":" +
                                              "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }

                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }

                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }


    }
}