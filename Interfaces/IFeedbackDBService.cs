using Webapi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Webapi.Request;
using Webapi.Response;

namespace Webapi.interfaces
{
    public interface IFeedbackDBServcie
    {
       
        /// <summary>
        /// 用Id查詢回饋
        /// </summary>
        /// <param name="MembersId">帳號</param>
        Task<WhichFeedbackRequest> GetFeedbackById(string Id);
        /// <summary>
        /// 管理者回覆一筆系統回饋
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="reply">回覆內容</param>
        Task ReplySystemFeedback(string MembersId, string reply);
        /// <summary>
        /// 取得所有系統評分
        /// </summary>
        Task<List<SystemFeedbackResponse>> GetAllSystemFeedbck();
        /// <summary>
        /// 新增回饋
        /// </summary>
        /// <param name="Data">回饋資料</param>+
        /// <param name="MembersData">會員資料</param>
        Task CreateFeedback(FeedbackRequest Data, Members MembersData);
        /// <summary>
        /// 修改回饋
        /// </summary>
        /// <param name="Data">回饋資料</param>
        /// <param name="MembersData">會員資料</param>
        Task UpdateFeedback(FeedbackRequest Data, Members MembersData);
    }
}