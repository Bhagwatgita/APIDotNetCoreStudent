using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DaoLayer.Persistence
{
    public static class ConnectionReader
    {
        public static DbConnection LoadJson()
        {
            DbConnection connecctionString = null;
            using (StreamReader r = new StreamReader(@"E:\Learning_Visual_Studio\APITEST\DaoLayer\ConnectionSettings.json"))
            {
                string json = r.ReadToEnd();
                connecctionString = JsonConvert.DeserializeObject<DbConnection>(json);
            }
            return connecctionString;
        }

        
    }
    public class DbConnection
    {
        public string ConnectionString;
    }
}
