using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;
using Microsoft.AspNetCore.Http;

namespace Webapi.interfaces
{
    public interface IImageFileDBServices
    {

        /// <summary>
        /// 新增ImageFile資料
        /// </summary>
        /// <param name="Data">ImageFile資料</param>
        Task InsertImageFile(ImageFile Data);
        /// <summary>
        /// 將上傳圖片儲存進image資料夾
        /// </summary>
        /// <param name="file">圖片檔案</param>
        /// <returns>filepath,filename</returns>
        Task<List<string>> CopyImageFile(IFormFile file);
        /// <summary>
        /// 更改檔案名稱
        /// </summary>
        /// <param name="Data">圖片資料表</param>
        /// <param name="filepath">檔案路徑</param>
        /// <returns>圖片資料表</returns>
        Task<ImageFile> renameImageFile(ImageFile Data,string filepath);
        /// <summary>
        /// 設定ImageFile資料
        /// </summary>
        /// <param name="MembersId">MembersId</param>
        /// <param name="pyfilename">在python分析出來的檔名</param>
        /// <returns>圖片資料表</returns>
        Task<ImageFile> SetRigthData(string MembersId,string pyfilename);
        /// <summary>
        /// 檢查分析次數是否已達成本次檢測最大次數
        /// </summary>
        /// <param name="count">本次檢測已分析次數</param>
        /// <returns>是否可以存進ImageFile資料表</returns>
        Task<bool> CheckImageFileIdCount(int count);
    }
}