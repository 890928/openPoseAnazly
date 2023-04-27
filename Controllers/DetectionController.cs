using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webapi.Models;
using Webapi.interfaces;
using Webapi.Request;
using Webapi.Response;
using Microsoft.AspNetCore.Authorization;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DetectionController : ControllerBase
    {
        private readonly OpenposeContext _openposeContext;
        private readonly IDetectionDBServices _detectionDBServices;
        private readonly IMembersDBServices _membersDBService;
        public DetectionController(
            OpenposeContext openposeContext,
            IDetectionDBServices detectionDBServices,
            IMembersDBServices membersDBService)
        {
            _openposeContext = openposeContext;
            _detectionDBServices = detectionDBServices;
            _membersDBService = membersDBService;
        }

        [HttpPost]      
        public async Task<IActionResult> GetAllData(GetAllHistoryRequest Data)
        {
            if (await _membersDBService.MembersIdCheck(Data.MemberId) != true)
            {
                var AllData = await _detectionDBServices.selectDetection(Data.MemberId);
                if (AllData.Count != 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Status = 200,
                        Data = AllData
                    });
                }
                else
                {
                    return Ok(new ApiResponse()
                    {
                        Status = 200,
                        ErrorMessage = "該用戶無資料"
                    });
                }

            }return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Start(StartandEndRequest Data)
        {
            if (await _membersDBService.MembersIdCheck(Data.MembersId) != true)
            {
                string detectionId = await _detectionDBServices.GetDetectionId(Data.MembersId, true);
                if (await _detectionDBServices.CheckDetectionIsStart(Data.MembersId))
                {
                    await _detectionDBServices.InsertStartDetection(Data.MembersId); //新增開始檢測資料
                    return Ok(new ApiResponse()
                    {
                        Status = 200,
                        Data = "成功"
                    });
                }
                return Ok(new ApiResponse()
                {
                    Status = 404,
                    ErrorMessage = "檢測已開始"
                });
            }
            return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }
        [HttpPost]
        public async Task<IActionResult> End(StartandEndRequest Data)
        {
            if (await _membersDBService.MembersIdCheck(Data.MembersId) != true)
            {
                string detectionId = await _detectionDBServices.GetDetectionId(Data.MembersId, false); //取得檢測Id
                if (await _detectionDBServices.CheckDetectionIsEnd(detectionId))
                {
                    if (await _detectionDBServices.CheckHaveCorrection(detectionId))
                    {
                        await _detectionDBServices.InsertEndDetection(detectionId, Data.MembersId); //將檢測資料寫入結果以及結束時間
                        return Ok(new ApiResponse()
                        {
                            Status = 200,
                            Data = "成功," + detectionId
                        });
                    }
                    await _detectionDBServices.DeleteDetection(detectionId); //刪除檢測資料
                    return Ok(new ApiResponse()
                    {
                        Status = 404,
                        ErrorMessage = "本次檢測尚未分析，已將本次檢測資料刪除"
                    });
                }
                return Ok(new ApiResponse()
                {
                    Status = 404,
                    ErrorMessage = "檢測尚未開始或是上次檢測已結束，故不允許結束"
                });
            }
            return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }
        [HttpPost]
        public async Task<IActionResult> GetDetectionDetail(DetectionDetailRequest Data)
        {
            if (await _membersDBService.MembersIdCheck(Data.MembersId) != true)
            {
                if (await _detectionDBServices.CheckDetectionIdisExist(Data.DetectionId, Data.MembersId))
                {
                    List<DetectionDetailResponse> resultData = await _detectionDBServices.GetResultandImagefile(Data.DetectionId, Data.MembersId);
                    return Ok(new ApiResponse()
                    {
                        Status = 200,
                        Data = resultData
                    });
                }
                return Ok(new ApiResponse()
                {
                    Status = 404,
                    ErrorMessage = "查無檢測Id"
                });
            }
            return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }
    }
}