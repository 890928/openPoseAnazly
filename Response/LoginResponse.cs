using System.Net;

namespace Webapi.Response
{
    public class LoginResponse
    {
        public int  Status { get; set; }
        public dynamic Data { get; set; }
        public dynamic Token{get;set;}
        public string MembersId{get;set;}
        public string Name{get;set;}
        public string ImageUrl{get;set;}
        public dynamic CorrectionErrorMessage{get;set;}
        public string ErrorMessage { get; set; }
    }
}