using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Webapi.Response;

namespace Webapi.Filters
{
    public class FileLimitAttribute : Attribute, IResourceFilter
    {
        public long Size = 1;
        public FileLimitAttribute(long size = 1)
        {
            Size = size;
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //throw new NotImplementedException();
        }
        public void OnResourceExecuting(ResourceExecutingContext context) //進入的
        {
            var files = context.HttpContext.Request.Form.Files; //讀取檔案
            if (files.Count.Equals(0))
            {
                context.Result = new JsonResult(new ApiResponse()
                {
                    Status = 400,
                    ErrorMessage = "請上傳圖片"
                });
            }
            foreach (var file in files)
            {
                if (file.Length > 1024 * 500 * Size)
                {
                    context.Result = new JsonResult(new ApiResponse()
                    {
                        Status = 400,
                        ErrorMessage = "檔案太大"
                    });
                }
                string fileType = Path.GetExtension(file.FileName);
                if ((fileType == ".jpg") || (fileType == ".png") || (fileType == ".jpeg"))
                {
                }
                else
                {
                    context.Result = new JsonResult(new ApiResponse()
                    {
                        Status = 400,
                        ErrorMessage = "這不是圖片格式"
                    });
                }
            }
        }
    }
}