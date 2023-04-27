using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;
using Webapi.Response;

namespace Webapi.interfaces
{
    public interface IDetectionDBServices
    {

        /// <summary>
        /// 以帳號取出歷程資料
        /// </summary>
        /// <param name="memberId">帳號</param>
        Task<List<Detection>> selectDetection(string memberId);

        /// <summary>
        /// 新增Detection開始資料
        /// </summary>
        /// <param name="MembersId">MembersId</param>
        Task InsertStartDetection(string MembersId);

        /// <summary>
        /// 加入Detection結束資料
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="membersId">帳號Id</param>
        Task InsertEndDetection(string detectionId, string membersId);

        /// <summary>
        /// 刪除Detection資料
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        Task DeleteDetection(string detectionId);

        /// <summary>
        /// 取得檢測結果
        /// </summary>
        /// <param name="np">不良率</param>
        Task<string> Getnp_result(int np);

        /// <summary>
        /// 取得檢測Id
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <param name="IsNew">是否為新檢測</param>
        Task<string> GetDetectionId(string MembersId, bool IsNew);

        /// <summary>
        /// 計算良率
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="membersId">帳號Id</param>
        /// <returns>良率</returns>
        Task<int> Getnp(string detectionId, string membersId);

        /// <summary>
        /// 確認是否有檢測
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        Task<bool> CheckHaveCorrection(string detectionId);
        /// <summary>
        /// 確認檢測是否開始
        /// </summary>
        /// <param name="MembersId">檢測Id</param>
        /// <returns>檢測是否開始</returns>
        Task<bool> CheckDetectionIsStart(string MembersId);
        /// <summary>
        /// 確認是否在檢測中
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        Task<bool> CheckDetectionIsEnd(string detectionId);
         /// <summary>
        /// 檢查是否有DetectionId
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="MembersId">MembersId</param>
        /// <returns>是否有檢測Id</returns>
        Task<bool> CheckDetectionIdisExist(string DetectionId,string MembersId);
        /// <summary>
        /// 用檢測Id以及帳號取得分析結果與分析圖
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="MembersId">MembersId</param>
        /// <returns>ResultandImagefile</returns>
        Task<List<DetectionDetailResponse>> GetResultandImagefile(string DetectionId,string MembersId);
    }
}