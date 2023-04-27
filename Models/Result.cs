using System;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class Result
    {
        [StringLength(36)]
        public string ResultId { get; set; }                //分析Id
        public DateTime time { get; set; }                  //分析的時間
        public int direction { get; set; }                  //視訊位置
        public bool neck_check { get; set; }                //檢查脖子姿勢是否正確
        public int elbow { get; set; }                      //手肘角度
        public bool elbow_check { get; set; }               //檢查手肘姿勢是否正確
        public int waist { get; set; }                      //腰部角度
        public bool waist_check { get; set; }               //檢查腰部姿勢是否正確
        public int knee { get; set; }                       //膝蓋角度
        public bool knee_check { get; set; }                //檢查膝蓋姿勢是否正確
        public bool result { get; set; }                    //綜合結果
        public string error { get; set; }                   //不良姿勢訊息
        public string ImageFileId { get; set; }             //分析圖片Id
        public string DetectionId { get; set; }             //檢測Id
        public virtual ImageFile ImageFile { get; set; }    //與ImageFile資料表關聯
        public virtual Detection Detection { get; set; }    //與Detection資料表關聯
    }
}
