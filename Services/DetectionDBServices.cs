using System;
using Webapi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Webapi.interfaces;
using Webapi.InversionOfControl;
using Webapi.enums;
using Webapi.Response;

namespace Webapi.services
{
    [RegisterIOC]
    public class DetectionDBServices : IDetectionDBServices
    {
        public readonly OpenposeContext _openposeContext;
        public readonly IPublicDBServices _publicDBServices;
        public DetectionDBServices(
            OpenposeContext openposeContext,
            IPublicDBServices publicDBServices
            )
        {
            _openposeContext = openposeContext;
            _publicDBServices = publicDBServices;
        }

        /// <summary>
        /// 以帳號取出歷程資料
        /// </summary>
        /// <param name="memberId">帳號</param>
        public async Task<List<Detection>> selectDetection(string memberId)
        {
            var filter = _openposeContext.Detection.Where(x => x.MembersId == memberId); //
            var selectData = await filter.Select(x => new Detection
            {
                DetectionId = x.DetectionId,
                start_time = x.start_time,
                end_time = x.end_time,
                np_result = x.np_result,
                np = x.np,
                MembersId = x.MembersId
            }).AsNoTracking().ToListAsync();
            return selectData;
        }

        /// <summary>
        /// 新增Detection開始資料
        /// </summary>
        /// <param name="MembersId">MembersId</param>
        public async Task InsertStartDetection(string MembersId)
        {
            var dbdetection = new Detection
            {
                DetectionId = await GetDetectionId(MembersId, true),
                start_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                MembersId = MembersId
            };
            _openposeContext.Detection.Add(dbdetection);
            await _openposeContext.SaveChangesAsync();
        }
        /// <summary>
        /// 加入Detection結束資料
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="membersId">帳號Id</param>
        public async Task InsertEndDetection(string detectionId, string membersId)
        {
            var dbdetection = await _openposeContext.Detection
                              .FindAsync(detectionId);
            dbdetection.end_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            dbdetection.np = await Getnp(detectionId, membersId);
            dbdetection.np_result = await Getnp_result(Convert.ToInt32(dbdetection.np));
            await _openposeContext.SaveChangesAsync();
        }
        /// <summary>
        /// 刪除Detection資料
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        public async Task DeleteDetection(string detectionId)
        {
            var dbdetection = await _openposeContext.Detection
                              .FindAsync(detectionId);
            _openposeContext.Remove(dbdetection);
            await _openposeContext.SaveChangesAsync();
        }
        /// <summary>
        /// 取得檢測結果
        /// </summary>
        /// <param name="np">不良率</param>
        /// <returns>檢測結果</returns>
        public async Task<string> Getnp_result(int np)
        {
            string result = string.Empty;
            switch (np)
            {
                case int i when (90 <= i && i <= 100):
                    result = "完美";
                    break;
                case int i when (60 <= i && i <= 89):
                    result = "優良";
                    break;
                case int i when (30 <= i && i <= 59):
                    result = "尚可";
                    break;
                case int i when (0 <= i && i <= 29):
                    result = "待加強";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 取得檢測Id
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <param name="IsNew">是否為新檢測</param>
        /// <returns>檢測Id</returns>
        public async Task<string> GetDetectionId(string MembersId, bool IsNew)
        {
            string DetectionId = string.Empty;
            string Date = DateTime.Now.ToString("yyyyMMdd");
            var Db = await _openposeContext.Detection.AsNoTracking().ToListAsync();
            if (IsNew)
            {
                int newdetectionIdnum = 0;
                var newdetectionId = Db.OrderByDescending(x => x.start_time)
                                        .FirstOrDefault(x => x.DetectionId.Contains(Date)); //尋找最新一筆的DetectionId
                if (newdetectionId != null)
                {
                    string[] newdetectionIdsplit = newdetectionId.DetectionId.ToString().Split("T"); //把DetectionId用T隔開
                    newdetectionIdnum = Convert.ToInt32(newdetectionIdsplit[1]); //陣列元素的第一個就是DeteciontId命名到底幾個
                }
                DetectionId = DateTime.Now.ToString("yyyyMMdd") + DataIdType.T + string.Format("{0:0000}", newdetectionIdnum + 1);
            }
            else
            {
                var detectionId = Db.OrderByDescending(x => x.DetectionId)
                                    .FirstOrDefault(x => x.MembersId == MembersId);
                if (detectionId != null)
                {
                    DetectionId = detectionId.DetectionId;
                }
                else
                {
                    DetectionId = null;
                }
            }
            return DetectionId;
        }
        /// <summary>
        /// 計算良率
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="membersId">帳號Id</param>
        /// <returns>良率</returns>
        public async Task<int> Getnp(string detectionId, string membersId)
        {
            float count = await _publicDBServices.Getcount(detectionId); //取得一次檢測的總分析數
            var dbresult = await _openposeContext.Result.AsNoTracking().ToListAsync();
            float nptrue = dbresult
                          .Where(x => x.DetectionId == detectionId)
                          .Count(x => x.result == true); //取得一次檢測的分析坐姿為False的個數
            int res = Convert.ToInt32(Math.Round((nptrue / count) * 100));
            return res;
        }
        /// <summary>
        /// 確認是否有檢測
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <returns>是否有檢測</returns>
        public async Task<bool> CheckHaveCorrection(string detectionId)
        {
            var dbresult = await _openposeContext.Result.AsNoTracking().ToListAsync();
            int count = dbresult.Count(x => x.DetectionId == detectionId);
            return (count != 0);
        }
        /// <summary>
        /// 確認檢測是否開始
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <returns>檢測是否開始</returns>
        public async Task<bool> CheckDetectionIsStart(string MembersId)
        {
            var date = DateTime.Now;
            var dbresult = await _openposeContext.Detection.AsNoTracking().ToListAsync();
            var isdetection = dbresult.OrderByDescending(x => x.start_time)
                              .FirstOrDefault(x => x.MembersId == MembersId && x.start_time <= date && x.end_time == (DateTime?)null); //開始時間排序找出MembersId相同開始時間比現在時間早結束時間是空的
            return (isdetection == null);
        }
        /// <summary>
        /// 確認是否在檢測中
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <returns>是否有在檢測中</returns>
        public async Task<bool> CheckDetectionIsEnd(string detectionId)
        {
            var date = DateTime.Now;
            var dbresult = await _openposeContext.Detection.AsNoTracking().ToListAsync();
            var isdetection = dbresult.FirstOrDefault(x => x.DetectionId == detectionId && x.start_time <= date && x.end_time == (DateTime?)null);
            return (isdetection != null);
        }
        /// <summary>
        /// 檢查是否有DetectionId
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="MembersId">MembersId</param>
        /// <returns>是否有檢測Id</returns>
        public async Task<bool> CheckDetectionIdisExist(string DetectionId, string MembersId)
        {
            var dbdetection = await _openposeContext.Detection.AsNoTracking().ToListAsync();
            var Data = dbdetection.FirstOrDefault(x => x.DetectionId == DetectionId && x.MembersId == MembersId);
            return (Data != null);
        }
        /// <summary>
        /// 用檢測Id以及帳號取得分析結果與分析圖
        /// </summary>
        /// <param name="detectionId">檢測Id</param>
        /// <param name="MembersId">MembersId</param>
        /// <returns>ResultandImagefile</returns>
        public async Task<List<DetectionDetailResponse>> GetResultandImagefile(string DetectionId, string MembersId)
        {
            var resultDB = await _openposeContext.Result.AsNoTracking().ToListAsync();
            var imagefileDB = await _openposeContext.ImageFile.AsNoTracking().ToListAsync();
            var Data = resultDB.Join(imagefileDB,
                        x => x.ImageFileId,
                        y => y.ImageFileId,
                        (x, y) => new DetectionDetailResponse
                        {
                            DetectionId = x.DetectionId,
                            time = x.time,
                            result = x.result,
                            neck_check = x.neck_check,
                            elbow = x.elbow,
                            elbow_check = x.elbow_check,
                            waist = x.waist,
                            waist_check = x.waist_check,
                            knee = x.knee,
                            knee_check = x.knee_check,
                            error = x.error,
                            imagefile =  y.image
                        }).OrderBy(xy => xy.time).Where(xy => xy.DetectionId == DetectionId).ToList();
            //將Result與ImageFile Join 以分析時間排序找尋檢測Id相同的
            foreach (var item in Data)
            {
                string[] path = item.imagefile.Split("\\");
                item.imagefile = ".\\" + path[path.Count() - 2] + "\\" + path[path.Count() - 1];
            }
            return Data;
        }
    }
}