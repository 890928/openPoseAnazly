using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Webapi.interfaces;
using Webapi.Request;
using Webapi.Response;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ResultController : ControllerBase
    {
        private readonly IPublicDBServices _publicDBServices;
        private readonly IDetectionDBServices _detectionDBServices;
        private readonly IResultDBServices _resultDBServices;
        private readonly IMembersDBServices _membersDBService;
        private readonly IImageFileDBServices _imageFileDBServices;
        public ResultController(
            IPublicDBServices publicDBServices,
            IDetectionDBServices detectionDBServices,
            IResultDBServices resultDBServices,
            IMembersDBServices membersDBService,
            IImageFileDBServices imageFileDBServices)
        {
            _publicDBServices = publicDBServices;
            _detectionDBServices = detectionDBServices;
            _resultDBServices = resultDBServices;
            _membersDBService = membersDBService;
            _imageFileDBServices = imageFileDBServices;
        }
        [HttpPost]
        public async Task<IActionResult> GetScreenshotFile(GetScreenshotIdRequest Data)
        {
            if (await _membersDBService.MembersIdCheck(Data.MembersId) != true)
            {
                string detectionId = await _detectionDBServices.GetDetectionId(Data.MembersId,false);
                if (await _detectionDBServices.CheckDetectionIdisExist(detectionId, Data.MembersId))
                {
                    if (await _detectionDBServices.CheckDetectionIsEnd(detectionId))
                    {
                        int count = await _publicDBServices.Getcount(detectionId); //取得要分析的次數
                        if (await _imageFileDBServices.CheckImageFileIdCount(count))
                        {
                            string ScreenshotId = Data.MembersId + (count + 1).ToString() + ".jpg"; //將MembersId與次數結合唯一個新的檔名
                            return Ok(new ApiResponse()
                            {
                                Status = 200,
                                Data = ScreenshotId
                            });
                        }
                        return Ok(new ApiResponse()
                        {
                            Status = 404,
                            ErrorMessage = "本次檢測的分析照片已滿，無法再存入資料庫，請結束檢測"
                        });
                    }
                    return Ok(new ApiResponse()
                    {
                        Status = 404,
                        ErrorMessage = "檢測已結束"
                    });
                }
                return Ok(new ApiResponse()
                {
                    Status = 404,
                    ErrorMessage = "查無檢測歷程"
                });
            }
            return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Correction(CorrectionRequest InsertData)
        {
            if (await _membersDBService.MembersIdCheck(InsertData.MembersId) != true)
            {
                string[] file = InsertData.Filename.Split(".");
                if (file[file.Count()-1] == "jpg")
                {
                    ResultRequest result = await _resultDBServices.SentRequestToPython(InsertData);
                    if (result != null)
                    {
                        if (result.ispeople == true)
                        {
                            if (InsertData.IsData)
                            {
                                if (await _resultDBServices.SetRigthDataAndInsertData(result, InsertData))
                                {
                                    if (result.result)
                                    {
                                        return Ok(new ApiResponse()
                                        {
                                            Status = 200,
                                            Data = result.result + "," + result.DetectionId
                                        });
                                    }
                                    else
                                    {
                                        result.error.RemoveAt(result.error.Count() - 1);
                                        return Ok(new ApiResponse()
                                        {
                                            Status = 200,
                                            Data = result.result + "," + result.DetectionId,
                                            CorrectionErrorMessage = result.error
                                        });
                                    }
                                }
                                return Ok(new ApiResponse()
                                {
                                    Status = 404,
                                    ErrorMessage = result.error[result.error.Count() - 1]
                                });
                            }
                            return Ok(new ApiResponse()
                            {
                                Status = 200,
                                Data = result.result,
                                ErrorMessage = "檢測圖片已刪除"
                            });
                        }
                        return Ok(new ApiResponse()
                        {
                            Status = 404,
                            ErrorMessage = result.error[0]
                        });
                    }
                    return Ok(new ApiResponse()
                    {
                        Status = 500,
                        ErrorMessage = "尚未鏈接到PythonAPI"
                    });
                }
                return Ok(new ApiResponse()
                {
                    Status = 404,
                    ErrorMessage = "副檔名必須要是jpg"
                });
            }
            return Ok(new ApiResponse()
            {
                Status = 404,
                ErrorMessage = "帳號尚未註冊"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage(Base64JsonRequest Data)
        {
            MemoryStream memStream = new MemoryStream(Data.Image64);
            Image mImage = Image.FromStream(memStream);
            var ImageId = Data.Name + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            var basePath = Environment.CurrentDirectory;
            var imagePath = "./wwwroot/Image";
            var filepath = Path.GetFullPath(imagePath, basePath) + "\\" + ImageId;
            Bitmap bp = new Bitmap(mImage);
            bp.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return Ok(new ApiResponse()
            {
                Status = 200,
                Data = filepath
            });
        }
    }
}
