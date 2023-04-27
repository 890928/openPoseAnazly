using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Webapi.Models;

namespace Webapi.Request
{
    public class RegisterRequest
    {
        [StringLength(200,ErrorMessage ="信箱長度請小於200字")]
        [Required(ErrorMessage = "請輸入信箱")]
        [EmailAddress(ErrorMessage = "這不是信箱的格式")]
        [DataType(DataType.EmailAddress)]
        public string MembersId { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
        [Required(ErrorMessage = "請輸入確認密碼")]
        public string Passwordcheck { get; set; }
    }
}