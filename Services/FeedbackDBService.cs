using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Threading.Tasks;
using Webapi.interfaces;
using Webapi.InversionOfControl;
using Webapi.Models;
using Microsoft.EntityFrameworkCore;
using Webapi.Request;
using Webapi.Response;

namespace Webapi.services
{
    [RegisterIOC]
    public class FeedbackDBServcie : IFeedbackDBServcie
    {
        private OpenposeContext _openposeContext { get; set; }
        public FeedbackDBServcie(OpenposeContext openposeContext)
        {
            _openposeContext = openposeContext;
        }
        /// <summary>
        /// 用Id查詢回饋
        /// </summary>
        /// <param name="MembersId">帳號</param>
        public async Task<WhichFeedbackRequest> GetFeedbackById(string Id)
        {
            WhichFeedbackRequest Data = new WhichFeedbackRequest();
            if (Id.Contains("@"))
            {
                Data.SystemFeedback = await _openposeContext.SystemFeedback.FindAsync(Id).ConfigureAwait(true);
            }
            else
            {
                Data.DetectionFeedback = await _openposeContext.DetectionFeedback.FindAsync(Id).ConfigureAwait(true);
            }
            return Data;
        }
        /// <summary>
        /// 回覆一筆回饋
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="reply">回覆內容</param>
        public async Task ReplySystemFeedback(string MembersId, string reply)
        {
            var Data = await _openposeContext.SystemFeedback.FindAsync(MembersId);
            Data.reply = reply;
            Data.reply_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            _openposeContext.SaveChanges();
        }
        /// <summary>
        /// 取得所有系統評分
        /// </summary>
        public async Task<List<SystemFeedbackResponse>> GetAllSystemFeedbck()
        {
            var SystemFeedbackDetailDB = _openposeContext.SystemFeedback.ToList()
            .Join(_openposeContext.Members.ToList(),x => x.MembersId,y => y.MembersId, (x, y) => new{x,y})
            .Join(_openposeContext.ImageFile.ToList(),xy => xy.y.ImageFileId,z => z.ImageFileId,
                                                    (xy, z) => new SystemFeedbackResponse
                                                    {
                                                        MembersId = xy.x.MembersId,
                                                        name = xy.x.name,
                                                        score = xy.x.score,
                                                        content = xy.x.content,
                                                        create_time = xy.x.create_time,
                                                        reply = xy.x.reply,
                                                        reply_time = xy.x.reply_time,
                                                        ImageFileId = xy.y.ImageFileId,
                                                        Image = z.image
                                                    }).OrderBy(xyz => xyz.create_time).ToList();
            return SystemFeedbackDetailDB;
        }
        /// <summary>
        /// 新增回饋
        /// </summary>
        /// <param name="Data">回饋資料</param>+
        public async Task CreateFeedback(FeedbackRequest Data, Members MembersData)
        {
            if (Data.DetectionId == null)
            {
                SystemFeedback DBData = new SystemFeedback();
                DBData.MembersId = MembersData.MembersId;
                DBData.name = MembersData.name;
                DBData.content = Data.Content;
                DBData.create_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                DBData.score = Data.Score;
                await _openposeContext.SystemFeedback.AddAsync(DBData);
                await _openposeContext.SaveChangesAsync();
            }
            else
            {
                DetectionFeedback DBData = new DetectionFeedback();
                DBData.DetectionId = Data.DetectionId;
                DBData.name = MembersData.name;
                DBData.content = Data.Content;
                DBData.create_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                DBData.score = Data.Score;
                await _openposeContext.DetectionFeedback.AddAsync(DBData);
                await _openposeContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 修改回饋
        /// </summary>
        /// <param name="Data">回饋資料</param>
        /// <param name="MembersData">會員資料</param>
        public async Task UpdateFeedback(FeedbackRequest Data, Members MembersData)
        {
            if (Data.DetectionId == null)
            {
                var DBData = await _openposeContext.SystemFeedback.FindAsync(MembersData.MembersId);
                DBData.content = Data.Content;
                DBData.create_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                DBData.score = Data.Score;
            }
            else
            {
                var DBData = await _openposeContext.DetectionFeedback.FindAsync(Data.DetectionId);
                DBData.content = Data.Content;
                DBData.create_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                DBData.score = Data.Score;
            }
            await _openposeContext.SaveChangesAsync();
        }
    }
}