using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class SystemFeedback
    {
        [StringLength(200)]
        [Key]
        public string MembersId { get; set; }
        [Range(1,5,ErrorMessage ="分數請介於1~5分")]
        public int score { get; set; }
        public string name { get; set; }
        [StringLength(1000, ErrorMessage = "回饋內容最多1000字元")]
        public string content { get; set; }
        [Required]
        public DateTime create_time { get; set; }
        [StringLength(1000, MinimumLength = 0, ErrorMessage = "回覆內容最多1000字元")]
        public string reply { get; set; }
        public DateTime? reply_time { get; set; }
        public virtual Members Members { get; set; }

    }
}
