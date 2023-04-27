using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;

namespace Webapi.interfaces
{
    public interface IMembersDBServices
    {

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="Data">會員資料</param>
        /// <param name="image">大頭貼</param>
        Task register(Members Data, string image);
        /// <summary>
        /// 雜湊密碼
        /// </summary>
        /// <param name="password">密碼</param>
        Task<string> HashPassword(string password);
        /// <summary>
        /// 用MembersId查詢一筆資料
        /// </summary>
        /// <param name="MembersId">帳號</param>
        Task<Members> GetDataByMembersId(string MembersId);
        /// <summary>
        /// 用ImageFileId查詢一筆資料
        /// </summary>
        /// <param name="ImageFileId">用Id找圖片路徑</param>
        Task<ImageFile> GetDataByImageId(string ImageFileId);
        /// <summary>
        /// 檢查帳號是否重複
        /// </summary>
        /// <param name="MembersId">帳號</param>
        Task<bool> MembersIdCheck(string MembersId);
        /// <summary>
        /// 信箱驗證
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="authcode">驗證碼</param>
        Task<string> Validate(string MembersId, string authcode);
        /// <summary>
        /// 計算當天有幾個帳號
        /// </summary>
        Task<int> CountMembers();
        /// <summary>
        /// 流水號產生
        /// </summary>
        /// <param name="ID">流水號</param>
        Task<string> MakeStrId(int ID);
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="password">密碼</param>
        Task<string> login(string MembersId, string password);
        /// <summary>
        /// 密碼確認
        /// </summary>
        /// <param name="Data">會員資料</param>
        /// <param name="Password">密碼</param>
        Task<bool> PasswordCheck(Members Data, string Password);
        /// <summary>
        /// 取得角色(管理員)
        /// </summary>
        /// <param name="MembersId">帳號</param>
        Task<string> GetRole(string MembersId);
        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="MembersId">會員帳號</param>
        /// <param name="newpassword">新的密碼</param>
        Task<string> changepassword(string MembersId, string newpassword);
        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="MembersId">會員帳號</param>
        /// <param name="image">更換頭貼的新路徑</param>
        Task<string> changeheadshot(string MembersId, string image);
        /// <summary>
        /// 取得帳號用的ImagefileId
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <returns>ImagefileId</returns>
        Task<string> GetMemberCreateImagefileId(string MembersId);
        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <returns>ImagefileId</returns>
        Task DeleteMemberData(string MembersId);
        /// <summary>
        /// 恢復帳號
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        Task RestoreMembers(string MembersId, string password ,string authcode);
    }
}