using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class FeedbackRequest
    {
        public string DetectionId {get;set;}
        [Required(ErrorMessage = "請輸入分數")]
        [Range(1,5,ErrorMessage ="分數請介於1~5")]
        public int Score { get; set; }
        [StringLength(1000, ErrorMessage = "回饋內容最多1000字元")]
        public string Content{get;set;}

    }
}