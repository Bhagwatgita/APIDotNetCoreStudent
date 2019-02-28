using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DaoLayer.Persistence
{
    public interface IApiServiceDao
    {
        //void Init(IDbConnection connection);
        void OpenConnection();
        void CloseConnection();
        string GetConnectionString();
        DataSet ExecuteDataset(string sql);
        DataTable ExecuteDataTable(string sql);
        DataRow ExecuteDataRow(string sql);
        String GetSingleResult(string sql);
        String FilterString(string strVal);
        String FilterQuoteNative(string strVal);
        string Encode(string strVal);
        String FilterStringNativeTrim(string strVal);
        String FilterStringNative(string strVal);
        string SingleQuoteToDoubleQuote(string strVal);
        String FilterQuote(string strVal);
        IDbResult ParseDbResult(DataTable dt);
        IDbResult ParseDbResult(string sql);
        DataTable GetTable2(string sql);
        string ParseData(string data);
        string AutoSelect(string str1, string str2);
        string ParseDate(string data);
        DataTable GetTable(string sql);
        void ExecuteProcedure(string spName, SqlParameter[] param);
        string DataTableToText(ref DataTable dt, string delemeter, Boolean includeColHeader);
        string DataTableToText(ref DataTable dt, string delemeter);
        string DataTableToHTML(ref DataTable dt, Boolean includeColHeader);
        string DataTableToHTML(ref DataTable dt);
        IDbResult TryParseSQL(string sql);
        DataTable DecodeLogData(DataTable logTable);
        string GetDataTemplete(DataTable dt);
        DataTable CreateDataTableFromLogData(string data, ArrayList defaultFields);
        void CreateDataRowFromLogData(ref DataRow row, string data);
        DataTable getTable(string sql);


    }
}
