using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Webapi.Request
{
    public class ChangeRequest
    {
        [Required(ErrorMessage = "請輸入舊密碼")]
        public string Password { get; set; }
        [Required(ErrorMessage = "請輸入新密碼")]
        public string Newpassword { get; set; }
        [Required(ErrorMessage = "請輸入新密碼確認")]
        [Compare("Newpassword",ErrorMessage ="確認失敗")]
        public string NewpasswordCheck { get; set; }

    }
}