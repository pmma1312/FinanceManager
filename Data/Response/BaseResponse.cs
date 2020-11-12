using System.Collections.Generic;
using System.Net;

namespace FinanceManager.Data.Response
{
    public class BaseResponse
    {
        public ResponseInfos Infos { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public bool HasErrors => Infos.Errors.Count > 0;
        public bool HasInfos => Infos.Infos.Count > 0;
        public bool HasMessages => Infos.Messages.Count > 0;
        public HttpStatusCode StatusCode { get; set; }

        public BaseResponse()
        {
            Infos = new ResponseInfos();
            Data = new Dictionary<string, object>();
            StatusCode = HttpStatusCode.OK;
        }
    }

    public class ResponseInfos
    {
        public List<string> Errors { get; set; }
        public List<string> Infos { get; set; }
        public List<string> Messages { get; set; }

        public ResponseInfos()
        {
            Errors = new List<string>();
            Infos = new List<string>();
            Messages = new List<string>();
        }
    }
}
