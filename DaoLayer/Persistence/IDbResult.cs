using System;
using System.Collections.Generic;
using System.Text;

namespace DaoLayer.Persistence
{
    public interface IDbResult
    {
        string ErrorCode { set; get; }

        string Msg { set; get; }

        string Id { set; get; }

        string Extra { set; get; }
        string Extra2 { get; set; }

        void SetMessage(string errorCode, string msg, string id);
        
    }
}
