﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace DaoLayer.Persistence
{
    public class ApiServiceDao : IApiServiceDao
    {
        private IDbConnection _connection;
        private IDbResult _dbResult;

        public  ApiServiceDao(IDbConnection connection,IDbResult dbResult)
        {
            //_connection = new SqlConnection(GetConnectionString());
            _connection = connection;
            _connection.ConnectionString = GetConnectionString();
            _dbResult = dbResult;
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Open();
        }
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
                this._connection.Close();
        }

        public string GetConnectionString()
        {
            return ConnectionReader.LoadJson().ConnectionString;
        }

        public DataSet ExecuteDataset(string sql)
        {
            var ds = new DataSet();
            SqlDataAdapter da;

            try
            {
                OpenConnection();
                da = new SqlDataAdapter(sql, (SqlConnection)_connection);

                da.Fill(ds);
                da.Dispose();
                CloseConnection();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da = null;
                CloseConnection();
            }
            return ds;
        }
        public DataTable ExecuteDataTable(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                return ds.Tables[0];
            }
        }

        public DataRow ExecuteDataRow(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return ds.Tables[0].Rows[0];
            }
        }
        public String GetSingleResult(string sql)
        {
            try
            {
                var ds = ExecuteDataset(sql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return "";

                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                CloseConnection();
            }
        }

        public String FilterString(string strVal)
        {
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";

            return str;
        }

        public String FilterQuoteNative(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = Encode(strVal.Trim());


            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "");

                str = str.Replace("/*", "");
                str = str.Replace("*/", "");

                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");

                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");

                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");

                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");


                str = str.Replace("<script", "");

            }
            else
            {
                str = "null";
            }
            return str;
        }

        public string Encode(string strVal)
        {

            var sb = new StringBuilder(HttpUtility.HtmlEncode(HttpUtility.JavaScriptStringEncode(strVal)));
            // Selectively allow  <b> and <i>
            sb.Replace("&lt;b&gt;", "<b>");
            sb.Replace("&lt;/b&gt;", "");
            sb.Replace("&lt;i&gt;", "<i>");
            sb.Replace("&lt;/i&gt;", "");
            return sb.ToString();

        }
        public String FilterStringNativeTrim(string strVal)
        {
            var str = FilterQuoteNative(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";
            else
                str = "";

            return str;
        }

        public String FilterStringNative(string strVal)
        {
            var str = FilterQuoteNative(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";

            return str;
        }

        public string SingleQuoteToDoubleQuote(string strVal)
        {
            strVal = strVal.Replace("\"", "");
            return strVal.Replace("'", "\"");
        }

        public String FilterQuote(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = strVal.Trim();

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "");

                str = str.Replace("/*", "");
                str = str.Replace("*/", "");

                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");

                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");

                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");

                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");


                str = str.Replace("<script", "");

            }
            else
            {
                str = "null";
            }
            return str;
        }

        public IDbResult ParseDbResult(DataTable dt)
        {
            var res = _dbResult;
            if (dt.Rows.Count > 0)
            {
                res.ErrorCode = dt.Rows[0][0].ToString();
                res.Msg = dt.Rows[0][1].ToString();
                res.Id = dt.Rows[0][2].ToString();
            }
            if (dt.Columns.Count > 3)
            {
                res.Extra = dt.Rows[0][3].ToString();
            }
            if (dt.Columns.Count > 4)
            {
                res.Extra2 = dt.Rows[0][4].ToString();
            }
            return res;
        }

        public IDbResult ParseDbResult(string sql)
        {
            return ParseDbResult(ExecuteDataset(sql).Tables[0]);
        }

        public DataTable GetTable2(string sql)
        {
            return ExecuteDataset(sql).Tables[1];
        }

        public string ParseData(string data)
        {
            return data.Replace("\"", "").Replace("'", "").Trim();
        }

        public string AutoSelect(string str1, string str2)
        {
            if (str1.ToLower() == str2.ToLower())
                return "selected=\"selected\"";

            return "";
        }

        public string ParseDate(string data)
        {
            data = FilterString(data);
            if (data.ToUpper() == "NULL")
                return data;
            data = data.Replace("'", "");
            var dateParts = data.Split('/');
            if (dateParts.Length < 3)
                return "NULL";
            var m = dateParts[0];
            var d = dateParts[1];
            var y = dateParts[2];

            return "'" + y + "-" + (m.Length == 1 ? "0" + m : m) + "-" + (d.Length == 1 ? "0" + d : d) + "'";

        }

        public DataTable GetTable(string sql)
        {
            var ds = new DataSet();
            SqlDataAdapter da;

            try
            {
                OpenConnection();
                da = new SqlDataAdapter(sql, (SqlConnection)_connection);

                da.Fill(ds);
                da.Dispose();
                CloseConnection();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da = null;
                CloseConnection();
            }
            return ds.Tables[0];
        }

        public void ExecuteProcedure(string spName, SqlParameter[] param)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(spName, (SqlConnection)_connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter p in param)
                {
                    command.Parameters.Add(p);
                }
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                CloseConnection();
            }
        }

        public string DataTableToText(ref DataTable dt, string delemeter, Boolean includeColHeader)
        {
            var sb = new StringBuilder();
            var del = "";
            var rowcnt = 0;
            if (includeColHeader)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append(del);
                    sb.Append(col.ColumnName);
                    del = delemeter;
                }
                rowcnt++;
            }

            foreach (DataRow row in dt.Rows)
            {
                if (rowcnt > 0)
                {
                    sb.AppendLine();
                }
                del = "";
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append(del);
                    sb.Append(row[col.ColumnName].ToString());
                    del = delemeter;
                }
                rowcnt++;
            }
            return sb.ToString();
        }

        public string DataTableToText(ref DataTable dt, string delemeter)
        {
            return DataTableToText(ref dt, delemeter, true);
        }

        public string DataTableToHTML(ref DataTable dt, Boolean includeColHeader)
        {
            var sb = new StringBuilder("<table>");

            if (includeColHeader)
            {
                sb.AppendLine("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append("<th>" + col.ColumnName + "</th>");
                }
                sb.AppendLine("</tr>");
            }

            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append("<td>" + row[col.ColumnName].ToString() + "</td>");

                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        public string DataTableToHTML(ref DataTable dt)
        {
            return DataTableToHTML(ref dt, true);
        }

        public IDbResult TryParseSQL(string sql)
        {
            var dr = _dbResult;
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = (SqlConnection)_connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SET NOEXEC ON " + sql + " SET NOEXEC OFF"; ;
                    command.ExecuteNonQuery();
                    dr.ErrorCode = "0";
                    dr.Msg = "Success";
                }
                return dr;
            }
            catch (Exception ex)
            {
                dr.ErrorCode = "1";
                dr.Msg = FilterQuote(ex.Message);
                return dr;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable DecodeLogData(DataTable logTable)
        {

            var data = GetDataTemplete(logTable);
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var fieldList = new ArrayList();
            fieldList.Add("Table");
            fieldList.Add("ChangedDate");
            fieldList.Add("ChangedBy");
            fieldList.Add("ChangedType");
            fieldList.Add("DataID");

            var dt = CreateDataTableFromLogData(data, fieldList);

            foreach (DataRow row in logTable.Rows)
            {
                DataRow newRow = dt.NewRow();
                newRow["Table"] = row["tableName"].ToString();
                newRow["ChangedDate"] = row["createdDate"].ToString();
                newRow["ChangedBy"] = row["createdBy"].ToString();
                newRow["ChangedType"] = row["logType"].ToString();
                newRow["DataID"] = row["dataId"].ToString();

                CreateDataRowFromLogData(ref newRow, row["newData"].ToString());
                dt.Rows.Add(newRow);
            }

            return dt;
        }

        #region Helper
        public string GetDataTemplete(DataTable dt)
        {
            var data = "";
            foreach (DataRow row in dt.Rows)
            {
                data = row["OldData"].ToString();
                if (string.IsNullOrWhiteSpace(data))
                {
                    data = row["OldData"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(data))
                {
                    return data;
                }
            }
            return data;
        }
        public DataTable CreateDataTableFromLogData(string data, ArrayList defaultFields)
        {
            var dt = new DataTable();

            foreach (var fld in defaultFields)
            {
                dt.Columns.Add(fld.ToString());
            }

            var stringSeparators = new[] { "-:::-" };
            var dataList = data.Split(stringSeparators, StringSplitOptions.None);
            const string seperator = "=";
            foreach (var itm in dataList)
            {
                var seperatorPos = itm.IndexOf(seperator);
                if (seperatorPos > -1)
                {
                    var field = itm.Substring(0, seperatorPos - 1).Trim();
                    dt.Columns.Add(field);
                }
            }
            return dt;
        }
        public void CreateDataRowFromLogData(ref DataRow row, string data)
        {
            var stringSeparators = new[] { "-:::-" };
            var dataList = data.Split(stringSeparators, StringSplitOptions.None);

            const string seperator = "=";
            foreach (var itm in dataList)
            {
                var seperatorPos = itm.IndexOf(seperator);
                if (seperatorPos > -1)
                {
                    var field = itm.Substring(0, seperatorPos - 1).Trim();
                    var value = itm.Substring(seperatorPos + 1).Trim();

                    row[field] = value;
                }
            }
        }

        public DataTable getTable(string sql)
        {
            return ExecuteDataTable(sql);
        }
        #endregion
    }
}
