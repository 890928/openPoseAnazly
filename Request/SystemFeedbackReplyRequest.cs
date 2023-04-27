using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class SystemFeedbackReplyRequest
    {
        [StringLength(200,ErrorMessage ="信箱長度請小於200字")]
        [EmailAddress(ErrorMessage = "這不是信箱的格式")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string MembersId{get;set;}
        [StringLength(1000, MinimumLength = 0, ErrorMessage = "回覆內容最多1000字元")]
        public string Reply{get;set;}
    }
}