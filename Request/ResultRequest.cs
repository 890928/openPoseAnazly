using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Request
{
    public class ResultRequest
    {
        [StringLength(36)]
        public string ResultId { get; set; }
        public int direction { get; set; }
        public bool neck_check {get;set;}
        public int elbow { get; set; }
        public bool elbow_check { get; set; }
        public int waist { get; set; }
        public bool waist_check { get; set; }
        public int knee { get; set; }
        public bool knee_check { get; set; }
        public bool ispeople { get; set; }
        public bool result { get; set; }
        public List<string> error { get; set; }
        public string ImageFileId { get; set; }
        public string DetectionId { get; set; }
        public string pyfilename { get; set; }
    }
}