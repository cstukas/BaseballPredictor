using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BaseballTracker.DB
{
    public static class Helper
    {
        private static string ConnectionString { get { return string.Format("Data Source={0}", Path.Combine(Environment.CurrentDirectory, @"..\..\..\BaseballDB.db")); } }

        public static T LoadObject<T>(string sql)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                T newObj = conn.QuerySingle<T>(sql);
                return newObj;
            }
        }

        public static void InsertObject<T>(T dto, string table)
        {
            List<Property> properties = Utils.GetAllValidProperties(dto);

            // build sql query
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO " + table + " ( ");
            //column names
            for (int i = 0; i < properties.Count; i++)
            {
                Property prop = properties[i];
                sqlBuilder.Append(prop.Name);
                if (i < properties.Count - 1) sqlBuilder.Append(",");
            }

            sqlBuilder.Append(") VALUES (");
            //values
            for (int i = 0; i < properties.Count; i++)
            {
                Property prop = properties[i];
                sqlBuilder.Append("'" + prop.Value + "'");
                if (i < properties.Count - 1) sqlBuilder.Append(",");
            }
            sqlBuilder.Append(")");

            string sql = sqlBuilder.ToString();

            // run query
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                {
                    db.Execute(sql, dto);
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdateObject<T>(T dto, T origDto, string table, string[] keys)
        {
            // get values to update
            List<Property> changedProperties = Utils.GetChangedProperties(dto, origDto);
            if (changedProperties.Count == 0) return;

            //get key values
            List<Property> keyProperties = new List<Property>();
            foreach (string key in keys)
            {
                keyProperties.Add(Utils.GetPropertyInfo(dto, key));
            }

            if (keyProperties.Count == 0)
                throw new Exception("Can't Update without a Key");

            // build sql query
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"UPDATE " + table + " SET ");
            for (int i = 0; i < changedProperties.Count; i++)
            {
                Property prop = changedProperties[i];
                sqlBuilder.Append(prop.Name + "='" + prop.Value + "'");
                if (i < changedProperties.Count - 1) sqlBuilder.Append(",");
            }
            sqlBuilder.Append(" WHERE ");
            for (int k = 0; k < keyProperties.Count; k++)
            {
                Property prop = keyProperties[k];
                sqlBuilder.Append(prop.Name + "='" + prop.Value + "'");
                if (k < keyProperties.Count - 1) sqlBuilder.Append(" AND ");
            }
            string sql = sqlBuilder.ToString();

            try
            {
                using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                {
                    db.Execute(sql, dto);
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> LoadList<T>(string select, string from, string orderBy)
        {
            List<T> list = new List<T>();
            try
            {
                if (orderBy.Length != 0)
                {
                    orderBy = " ORDER BY " + orderBy;
                }

                using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                {
                    string sql = "SELECT " + select + " FROM " + from + orderBy;
                    list = db.Query<T>(sql).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        public static List<T> LoadList<T>(string sql)
        {
            List<T> list = new List<T>();
            try
            {
                using (IDbConnection db = new SQLiteConnection(ConnectionString))
                {
                    list = db.Query<T>(sql).ToList();
                }
            }
            catch (Exception)
            {
                throw;

            }
            return list;
        }



        //used to establish initial connection, isnt really needed if we are loading anything else in 
        public static int TestConnection()
        {
            string sql = "SELECT 1;";
            try
            {
                using (IDbConnection conn = new SQLiteConnection(ConnectionString))
                {
                    int result = (Int32)conn.ExecuteScalar(sql);
                    return result;

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public static int GetDBValueCount(string table, string column, string value)
        {
            string sql = "SELECT COUNT(" + column + ") FROM " + table + " WHERE " + column + " = '" + value + "';";
            try
            {
                using (IDbConnection conn = new SQLiteConnection(ConnectionString))
                {
                    int count = (Int32)conn.ExecuteScalar(sql);
                    return count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int GetNextGameKey()
        {
            int key = -1;

            string sql = "SELECT NextKey FROM NextKeyTbl WHERE Id = 1;";
            try
            {
                using (IDbConnection conn = new SQLiteConnection(ConnectionString))
                {
                    key = conn.QuerySingle<int>(sql);
                }
            }
            catch (Exception)
            {
                throw;
            }

            int nextKey = key + 1;

            // Update table            
            string updateSql = "UPDATE NextKeyTbl Set NextKey = " + nextKey + " WHERE Id = 1";
            bool updateSuccess = false;
            try
            {
                using (IDbConnection db = new SQLiteConnection(ConnectionString))
                {
                    db.Execute(updateSql);
                    updateSuccess = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (updateSuccess)
                return nextKey;
            else
                return -1;
        }

        public static void ClearTable(string table, string where)
        {
            string sql = "DELETE FROM " + table + " " + where;
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(ConnectionString))
                {
                    db.Execute(sql);
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

    }



}

