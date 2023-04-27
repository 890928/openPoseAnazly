using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class LoginRequest
    {
        [StringLength(200,ErrorMessage ="信箱長度請小於200字")]
        [EmailAddress(ErrorMessage = "這不是信箱的格式")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string MembersId { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}