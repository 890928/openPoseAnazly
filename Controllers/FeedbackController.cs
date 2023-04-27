using Microsoft.AspNetCore.Mvc;
using Webapi.Models;
using Webapi.interfaces;
using System.Threading.Tasks;
using Webapi.Request;
using Webapi.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FeedbackController : ControllerBase
    {
        private readonly OpenposeContext _openposeContext;
        private readonly IMembersDBServices _membersDBServices;
        private readonly IFeedbackDBServcie _feedbackDBServcie;

        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IDetectionDBServices _detectionDBServices;
        public FeedbackController(
            OpenposeContext openposeContext,
            IMembersDBServices membersDBService,
            IFeedbackDBServcie feedbackDBServcie,
            IHttpContextAccessor httpContextAccessor,
            IDetectionDBServices detectionDBServices
            )
        {
            _openposeContext = openposeContext;
            _membersDBServices = membersDBService;
            _feedbackDBServcie = feedbackDBServcie;
            _httpContextAccessor = httpContextAccessor;
            _detectionDBServices = detectionDBServices;

        }
        /// <summary>
        /// 取得所有系統回饋
        /// </summary>
        public async Task<IActionResult> GetAllSystemFeedback()
        {
            List<SystemFeedbackResponse> Data = await _feedbackDBServcie.GetAllSystemFeedbck();
            foreach(var result in Data){
                string[] imageFileUrl = result.Image.Split(@"\");
                int a =imageFileUrl.Length;
                result.Image = imageFileUrl[a-2]+@"\"+imageFileUrl[a-1];
            }
            return Ok(new ApiResponse
            {
                Status = 200,
                Data = Data
            });
        }
        /// <summary>
        /// 創建系統回饋
        /// </summary>
        /// <param name="Data">回饋資料</param>
        [HttpPost]
        public async Task<IActionResult> CreateSystemFeedback(FeedbackRequest Data)
        {
            var MembersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Members MembersData = await _membersDBServices.GetDataByMembersId(MembersId);
            WhichFeedbackRequest CreateData = await _feedbackDBServcie.GetFeedbackById(MembersId);
            if (CreateData.SystemFeedback == null)
            {
                await _feedbackDBServcie.CreateFeedback(Data, MembersData);
                return Ok(new ApiResponse
                {
                    Status = 200,
                    Data = "已收到您的系統回饋"
                });
            }
            else
            {
                await _feedbackDBServcie.UpdateFeedback(Data, MembersData);
                return Ok(new ApiResponse
                {
                    Status = 200,
                    Data = "已更新您的系統回饋"
                });
            }
        }
        /// <summary>
        /// 管理者回覆系統回饋
        /// </summary>
        /// <param name="MembersId">回饋帳號</param>
        /// <param name="Data">回覆內容</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReplySystemFeedback(SystemFeedbackReplyRequest Data)
        {
            WhichFeedbackRequest replyData = await _feedbackDBServcie.GetFeedbackById(Data.MembersId);
            if (replyData != null)
            {
                replyData.SystemFeedback.reply = Data.Reply;
                await _feedbackDBServcie.ReplySystemFeedback(Data.MembersId, Data.Reply);
                if (string.IsNullOrEmpty(replyData.SystemFeedback.reply))
                {
                    return Ok(new ApiResponse
                    {
                        Status = 200,
                        Data = "已回覆會員：" + Data.MembersId + "的回饋"
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Status = 200,
                        Data = "已更新回覆會員：" + Data.MembersId + "的回饋"
                    });
                }
            }
            else
            {
                return Ok(new ApiResponse
                {
                    Status = 200,
                    ErrorMessage = "查無"+Data.MembersId +"的留言"
                });
            }
        }
        /// <summary>
        /// 檢測回饋
        /// </summary>
        /// <param name="DetectionId">檢測編號</param>
        /// <param name="Data">檢測回饋</param>
        [HttpPost]
        public async Task<IActionResult> DetectionFeedback(FeedbackRequest Data)
        {
            var MembersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Members MembersData = await _membersDBServices.GetDataByMembersId(MembersId);
            WhichFeedbackRequest detectionFeedbackData = await _feedbackDBServcie.GetFeedbackById(Data.DetectionId);
            if (await _detectionDBServices.CheckDetectionIdisExist(Data.DetectionId, MembersId))
            {
                if (await _detectionDBServices.CheckDetectionIsEnd(Data.DetectionId))
                {
                    if (detectionFeedbackData.DetectionFeedback == null)
                    {
                        await _feedbackDBServcie.CreateFeedback(Data, MembersData);
                        return Ok(new ApiResponse
                        {
                            Status = 200,
                            Data = "已收到您對檢測編號：" + Data.DetectionId + "的檢測回饋"
                        });

                    }
                    else
                    {
                        await _feedbackDBServcie.UpdateFeedback(Data, MembersData);
                        return Ok(new ApiResponse
                        {
                            Status = 200,
                            Data = "已更新您對檢測編號：" + Data.DetectionId + "的檢測回饋"
                        });
                    }
                }
                return Ok(new ApiResponse
                {
                    Status = 404,
                    ErrorMessage = "檢測尚未結束"
                });
            }
            return Ok(new ApiResponse
            {
                Status = 404,
                ErrorMessage = "您沒有：" + Data.DetectionId + "的檢測"
            });
        }
    }
}
