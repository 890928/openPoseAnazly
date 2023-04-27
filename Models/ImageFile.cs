using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class ImageFile
    {
        [StringLength(16)]
        public string ImageFileId { get; set; }
        [StringLength(1000)]
        public string image { get; set; }
    }
}
