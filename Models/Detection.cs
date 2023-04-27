using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Webapi.Models
{
    public class Detection
    {
        [StringLength(16)]
        [Key]
        public string DetectionId { get; set; }
        public DateTime start_time { get; set; }
        public DateTime? end_time { get; set; }
        [StringLength(10)]
        public string np_result { get; set; }
        public int? np { get; set; }
        public string MembersId { get; set; }
        public virtual Members Members { get; set; }
        public virtual DetectionFeedback DetectionFeedback { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
