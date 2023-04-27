using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapi.Request
{
    public class CorrectionRequest
    {
        [Required(ErrorMessage = "請輸入帳號")]
        public string MembersId{get;set;}
        [Required(ErrorMessage = "請輸入是否要寫進資料庫")]
        public bool IsData{get;set;}
        [Required(ErrorMessage = "請輸入圖片名稱")]
        public string Filename { get; set; }
        [Required(ErrorMessage = "請輸入檔案路徑")]
        public string Filepath { get; set; }
    }
}