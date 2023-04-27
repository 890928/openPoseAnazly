using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class DetectionFeedback
    {
        [StringLength(16)]
        [Key]
        public string DetectionId { get; set; }
        public string name { get; set; }
        public int score { get; set; }
        [StringLength(1000, MinimumLength = 0, ErrorMessage = "回覆內容最多1000字元")]
        public string content { get; set; }
        public DateTime create_time { get; set; }
        public virtual Detection Detection{ get; set; }
    }
}
