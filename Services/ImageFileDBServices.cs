using System;
using Webapi.Models;
using System.Threading.Tasks;
using Webapi.interfaces;
using Webapi.InversionOfControl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Webapi.enums;

namespace Webapi.services
{
    [RegisterIOC]
    public class ImageFileDBServices : IImageFileDBServices
    {
        private readonly OpenposeContext _openposeContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDetectionDBServices _detectionDBServices;
        private readonly IPublicDBServices _publicDBServices;
        public ImageFileDBServices(
            OpenposeContext openposeContext,
            IWebHostEnvironment webHostEnvironment,
            IDetectionDBServices detectionDBServices,
            IPublicDBServices publicDBServices
            )
        {
            _openposeContext = openposeContext;
            _webHostEnvironment = webHostEnvironment;
            _detectionDBServices = detectionDBServices;
            _publicDBServices = publicDBServices;
        }
        /// <summary>
        /// 新增ImageFile資料
        /// </summary>
        /// <param name="Data">ImageFile資料</param>
        public async Task InsertImageFile(ImageFile Data)
        {
            var dbimageFile = new ImageFile
            {
                ImageFileId = Data.ImageFileId,
                image = Data.image
            };
            _openposeContext.ImageFile.Add(dbimageFile);
            await _openposeContext.SaveChangesAsync();
        }
        /// <summary>
        /// 將上傳圖片儲存進image資料夾
        /// </summary>
        /// <param name="file">圖片檔案</param>
        /// <returns>filepath,filename</returns>
        public async Task<List<string>> CopyImageFile(IFormFile file)
        {
            string filepath = _webHostEnvironment.ContentRootPath + @"\image\"; //取得檔案資料夾的絕對路徑
            string filename = file.FileName;
            try
            {
                using (var filestream = System.IO.File.Create(filepath + filename))
                {
                    await file.CopyToAsync(filestream);
                }
                return await Task.FromResult(new List<string>
                {
                    filepath, filename
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return await Task.FromResult(new List<string>{
                "檔案存取失敗"
            });
        }
        /// <summary>
        /// 更改檔案名稱
        /// </summary>
        /// <param name="Data">圖片資料表</param>
        /// <param name="filepath">檔案路徑</param>
        /// <returns>圖片資料表</returns>
        public async Task<ImageFile> renameImageFile(ImageFile Data, string filepath)
        {
            string newfilename = filepath + Data.ImageFileId + ".jpg";
            System.IO.File.Move(Data.image, newfilename); //將原本的檔名取代成新的檔名
            Data.image = newfilename;
            return Data;
        }
        /// <summary>
        /// 設定ImageFile資料
        /// </summary>
        /// <param name="MembersId">MembersId</param>
        /// <param name="pyfilename">在python分析出來的檔名</param>
        /// <returns>圖片資料表</returns>
        public async Task<ImageFile> SetRigthData(string MembersId, string pyfilename)
        {
            ImageFile Data = new ImageFile();
            string DetectionId = await _detectionDBServices.GetDetectionId(MembersId, false); //取得檢測Id
            Data.ImageFileId = DetectionId + DataIdType.P + string.Format("{0:00}", (await _publicDBServices.Getcount(DetectionId) + 1));
            string filepath = _webHostEnvironment.ContentRootPath + @"\wwwroot\image\"; //取得檔案資料夾的絕對路徑
            Data.image = filepath + pyfilename;
            return Data;
        }
        /// <summary>
        /// 檢查分析次數是否已達成本次檢測最大次數
        /// </summary>
        /// <param name="count">本次檢測已分析次數</param>
        /// <returns>是否可以存進ImageFile資料表</returns>
        public async Task<bool> CheckImageFileIdCount(int count)
        {
            return (count < 99); //因為Id命名關係 故只能存99 而個數算出來只是資料庫裡的所以算98
        }
    }
}
