using System.ComponentModel.DataAnnotations;

namespace Webapi.enums
{
    public enum DataIdType
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        T,
        /// <summary>
        /// 檢測照片
        /// </summary>
        [Display(Name = "分析照片")]
        P,
        /// <summary>
        /// 大頭貼
        /// </summary>
        [Display(Name = "大頭照")]
        H,
    }
}