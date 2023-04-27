using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class ChangeheadshotRequest
    {
        public string Headshot_Base64 { get; set; }
    }
}