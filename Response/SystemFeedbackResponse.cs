using System;

namespace Webapi.Response
{
    public class SystemFeedbackResponse
    {
        public string MembersId { get; set; }
        public int score { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public DateTime create_time { get; set; }
        public string reply { get; set; }
        public DateTime? reply_time { get; set; }
        public string ImageFileId{get;set;}
        public string Image{get;set;}
    }
}