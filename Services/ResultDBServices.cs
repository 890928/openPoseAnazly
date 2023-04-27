using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.interfaces;
using Webapi.Request;
using System.Net;
using Webapi.InversionOfControl;
using Microsoft.AspNetCore.Hosting;
using Webapi.enums;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Webapi.services
{
    [RegisterIOC]
    public class ResultDBServices : IResultDBServices
    {
        public readonly OpenposeContext _openposeContext;
        public readonly IImageFileDBServices _imageFileDBServices;
        private readonly IDetectionDBServices _detectionDBServices;
        private readonly IPublicDBServices _publicDBServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ResultDBServices(OpenposeContext openposeContext,
                                IImageFileDBServices imageFileDBServices,
                                IDetectionDBServices detectionDBServices,
                                IPublicDBServices publicDBServices,
                                IWebHostEnvironment webHostEnvironment
        )
        {
            _openposeContext = openposeContext;
            _imageFileDBServices = imageFileDBServices;
            _detectionDBServices = detectionDBServices;
            _publicDBServices = publicDBServices;
            _webHostEnvironment = webHostEnvironment;
        }
        /// <summary>
        /// 將檢測照片送到python去分析
        /// </summary>
        /// <param name="CorrectiontData">CorrectionRequest</param>
        /// <returns>分析結果ResultRequest</returns>
        public async Task<ResultRequest> SentRequestToPython(CorrectionRequest CorrectiontData)
        {
            string inputval = "?" + "&MembersId=" + CorrectiontData.MembersId
                                  + "&imgpath=" + CorrectiontData.Filepath //圖片路徑
                                  + "&img=" + CorrectiontData.Filename //圖檔名
                                  + "&isData=" + CorrectiontData.IsData; //本次分析是否要存
            string url = "http://openposesystem.sytes.net:5000" + "/pose" + inputval; //PyWebAPI
            WebRequest wRequest = WebRequest.Create(url); //建立pywebapi請求
            wRequest.Method = "GET"; //請求方法為Get
            wRequest.ContentType = "text/json;charset=UTF-8"; //請求方法為Json格式
            string Jsonresult = string.Empty;
            try
            {
                using (WebResponse wResponse = wRequest.GetResponse()) //取得pywebapi串流
                {
                    using (Stream stream = wResponse.GetResponseStream()) //取得pywebapi回傳值
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8); //讀取串流值
                        Jsonresult = reader.ReadToEnd();  //url返回的值
                    } 
                } 
            }
            catch
            {
                return null;
            }
            return (JsonConvert.DeserializeObject<ResultRequest>(Jsonresult));
        }
        /// <summary>
        /// 設定ImageFile和Result的資料
        /// </summary>
        /// <param name="resultData">PyAPI傳回的Json資料</param>
        /// <param name="imageData">圖片資料</param>
        public async Task<bool> SetRigthDataAndInsertData(ResultRequest resultData, CorrectionRequest imageData)
        {
            string[] resultDone = { "" };
            resultData.DetectionId = await _detectionDBServices.GetDetectionId(imageData.MembersId, false); //取得檢測Id
            if (resultData.DetectionId != null && await _detectionDBServices.CheckDetectionIsEnd(resultData.DetectionId))
            {
                int DetectionIdCount = await _publicDBServices.Getcount(resultData.DetectionId);
                if (await _imageFileDBServices.CheckImageFileIdCount(DetectionIdCount))
                {
                    resultData.ImageFileId = resultData.DetectionId + DataIdType.P + string.Format("{0:00}", DetectionIdCount + 1);
                    ImageFile imageFile = await _imageFileDBServices.SetRigthData(imageData.MembersId, resultData.pyfilename);
                    imageFile = await _imageFileDBServices.renameImageFile(imageFile, imageData.Filepath);
                    await _imageFileDBServices.InsertImageFile(imageFile);
                    Guid res = Guid.NewGuid();
                    resultData.ResultId = res.ToString();
                    string errors = string.Empty;
                    if (!resultData.error.Count.Equals(0))
                    {
                        foreach (var err in resultData.error)
                        {
                            if (string.IsNullOrWhiteSpace(errors))
                            {
                                errors = err;
                            }
                            else
                            {
                                errors += "," + err;
                            }
                        }
                        resultData.error.Add(errors);
                    }
                    else
                    {
                        resultData.error.Add("null");
                    }
                    await Insert(resultData);
                    return true;
                }
                else
                {
                    System.IO.File.Delete(imageData.Filepath + resultData.pyfilename);
                    resultData.error.Add("本次檢測的分析照片已滿，無法再存入資料庫，請結束檢測");
                    return false;
                }
            }
            else
            {
                resultData.error.Add("尚未開始檢測或是檢測已結束");
                return false;
            }
        }
        /// <summary>
        /// 新增Result資料
        /// </summary>
        /// <param name="Data">Result資料</param>
        public async Task Insert(ResultRequest Data)
        {
            var dbresult = new Result
            {
                ResultId = Data.ResultId,
                time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                direction = Data.direction,
                neck_check = Data.neck_check,
                elbow = Data.elbow,
                elbow_check = Data.elbow_check,
                waist = Data.waist,
                waist_check = Data.waist_check,
                knee = Data.knee,
                knee_check = Data.knee_check,
                ImageFileId = Data.ImageFileId,
                DetectionId = Data.DetectionId,
                result = Data.result,
                error = Data.error[Data.error.Count() - 1]
            };
            _openposeContext.Result.Add(dbresult);
            await _openposeContext.SaveChangesAsync();
        }
    }
}