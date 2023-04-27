using System.ComponentModel.DataAnnotations;
using Webapi.Models;

namespace Webapi.Request
{
    public class WhichFeedbackRequest
    {
        public SystemFeedback SystemFeedback { get; set; } = new SystemFeedback();
        public DetectionFeedback DetectionFeedback { get; set; } = new DetectionFeedback();
    }
}