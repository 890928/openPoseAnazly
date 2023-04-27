using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class Base64JsonRequest
    {
        public byte[] Image64 { get; set; }

        public string Name {get;set;}
        
    }
}