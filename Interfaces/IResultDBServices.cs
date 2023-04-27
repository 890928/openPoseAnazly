using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;
using Webapi.Request;

namespace Webapi.interfaces
{
    public interface IResultDBServices
    {
        /// <summary>
        /// 將檢測照片送到python去分析
        /// </summary>
        /// <param name="CorrectiontData">CorrectionRequest</param>
        /// <returns>分析結果ResultRequest</returns>
        Task<ResultRequest> SentRequestToPython(CorrectionRequest CorrectiontData);

        /// <summary>
        /// 設定ImageFile和Result的資料
        /// </summary>
        /// <param name="resultData">PyAPI傳回的Json資料</param>
        /// <param name="imageData">圖片資料</param>
        Task<bool> SetRigthDataAndInsertData(ResultRequest resultData, CorrectionRequest imageData);

        /// <summary>
        /// 新增Result資料
        /// </summary>
        /// <param name="Data">Result資料</param>
        Task Insert(ResultRequest Data);
    }
}