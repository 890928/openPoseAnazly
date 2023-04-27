using System.Net;

namespace Webapi.Response
{
    public class ApiResponse
    {
        public int  Status { get; set; }
        public dynamic Data { get; set; }
        public dynamic CorrectionErrorMessage{get;set;}
        public string ErrorMessage { get; set; }
    }
}