using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionToSelectQuery
{
    public class AdoNetContext
    {
        public AdoNetContext()
        {
            if (DatabaseConnectionParameters.sqlConnectionString == null)
            {
                Configuration();
            }
        }

        public Entity Set<Entity>(Expression expression) where Entity : new()
        {
            var expressionToSqlSelect = new ExpressionToSqlSelect();
            string sqlCommandString = expressionToSqlSelect.GetSqlSelectString(expression);
            if (sqlCommandString == null)
            {
                throw new Exception("Select command not found");
            }
            using (var connection = new SqlConnection(DatabaseConnectionParameters.sqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlCommandString, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        return GetEntity<Entity>(dataTable);
                    }
                }
            }
            return new Entity();
        }


        private Entity GetEntity<Entity>(DataTable dataTable) where Entity : new()
        {
            if (dataTable.Rows.Count == 1)
            {
                var columnNames = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                Entity entity = new Entity();
                Type type = entity.GetType();
                IList<PropertyInfo> propertyInfos = new List<PropertyInfo>(type.GetProperties());
                CheckIfTypeIsMatching(columnNames, propertyInfos);

                foreach (var prop in propertyInfos)
                {
                    if (dataTable.Rows[0][prop.Name] != DBNull.Value)
                    {
                        PropertyMapHelper.ParsePrimitive(prop, entity, dataTable.Rows[0][prop.Name]);
                    }
                }

                return entity;
            }
            else if (dataTable.Rows.Count > 1)
            {
                throw new Exception("More than one row returned");
            }
            return new Entity();
        }

        private void CheckIfTypeIsMatching(List<string> columnNames, IList<PropertyInfo> propertyInfos)
        {
            if (columnNames.Count != propertyInfos.Count)
            {
                throw new Exception("Database tabel is not matching");
            }

            for (int i = 0; i < propertyInfos.Count; i++)
            {
                if (columnNames[i].Trim().ToLower() != propertyInfos[i].Name.Trim().ToLower())
                {
                    throw new Exception("Database tabel is not matching");
                }
            }
        }

        private static void Configuration()
        {           
            //UseSql("Server=NURLAN_B;Database=Northwind;User Id=sa1;Password=2");
        }
        public static void TableSet<Entity>(string dbTabelName)
        {
            DictionaryBase.tableNameHolder.Add(typeof(Entity).Name, dbTabelName);
        }
        public static void UseSql(string sqlConnectionString)
        {
            DatabaseConnectionParameters.sqlConnectionString = sqlConnectionString;
        }
    }
}
