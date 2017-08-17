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
    /// DataTable��չ, 
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        ///  ת��ΪDataView ����ʹ��������,��Captionʹ��DescriptionAttribute
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <param name="items">����</param>
        /// <param name="propertyNames"> </param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, params string[] propertyNames)
        {
            Type type = typeof(T);
            return ToDataTable((IEnumerable<Object>)items, type, propertyNames);
        }

        /// <summary>
        ///  ת��ΪDataView ����ʹ��������,��Captionʹ��DescriptionAttribute
        /// </summary>
        /// <param name="singleType">Ԫ������</param>
        /// <param name="items">����</param>
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
        /// ��ȡ��ṹ, column Nameʹ��������, Captionʹ��Description, û��Description��ʹ��������
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyTypeNames">�����򵼳�ȫ������</param>
        /// <returns></returns>
        public static DataTable GetTableStruct(Type type, params string[] propertyTypeNames)
        {
            //propertyTypeNames.NullCheck("propertyTypeNames");
            //propertyTypeNames.Any().FalseThrow("propertyTypeNames����Ҫ��һ��");

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
        /// ����ʵ�����������ֶε�Descrition������name��Caption (��Ҫ��;�ǰ�excel�е������еı���ʵ��)
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
                    attrs.ContainsKey(attr.Description).TrueThrow<ArgumentNullException>("����[{0}]��[{1}]�ظ�", dataType.FullName, attr.Description);
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
        /// ת�ñ��
        /// </summary>
        /// <param name="sourceTable">Դ</param>
        /// <param name="distinctColumnNamme">ת�ú������</param>
        /// <returns>ת�ú���</returns>
        public static DataTable TransPosition(this DataTable sourceTable, string distinctColumnNamme)
        {
            DataTable table = new DataTable();
            table.Columns.Add(distinctColumnNamme, typeof(string));
            //ָ���е������ݱ����
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                table.Columns.Add(sourceTable.Rows[i][distinctColumnNamme].ToString());
            }
            //ѭ��ÿ�б����
            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                var row = table.NewRow();

                string sourceColumnName = sourceTable.Columns[i].ColumnName;
                if (sourceColumnName == distinctColumnNamme) continue;

                row[distinctColumnNamme] = sourceColumnName;
                //ѭ��ÿ��
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
        /// ����JSON�ַ���
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