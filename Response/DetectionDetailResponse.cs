using System;

namespace Webapi.Response
{
    public class DetectionDetailResponse
    {
        public string DetectionId { get; set; }
        public DateTime time { get; set; }
        public bool result { get; set; }
        public bool neck_check{get;set;}
        public int elbow { get; set; }
        public bool elbow_check { get; set; }
        public int waist { get; set; }
        public bool waist_check { get; set; }
        public int knee { get; set; }
        public bool knee_check { get; set; }
        public string error{get;set;}
        public string imagefile { get; set; }
    }
}