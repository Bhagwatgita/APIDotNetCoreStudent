using System;
using System.Collections.Generic;
using System.Text;

namespace DaoLayer.Persistence
{
    public class DbResult : IDbResult
    {
        
        public string ErrorCode { set; get; }

        public string Msg { set; get; }

        public string Id { set; get; }

        public string Extra { set; get; }
        public string Extra2 { get; set; }

        public void SetMessage(string errorCode, string msg, string id)
        {
            ErrorCode = errorCode;
            Msg = msg;
            Id = id;
        }
    }
}
