using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class Members
    {
        [StringLength(200)]
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string MembersId { get; set; }
        [StringLength(10000)]
        [Required]
        public string password { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(10)]
        public string authcode { get; set; }
        public bool isadmin { get; set; }
        public bool DeleteOrNo{get;set;}
        public string ImageFileId { get; set; }
        public virtual ImageFile ImageFile { get; set; }
        public virtual SystemFeedback SystemFeedback{get;set;}
    }
}
