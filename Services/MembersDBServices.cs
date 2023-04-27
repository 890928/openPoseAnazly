using System.IO;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Webapi.Models;
using System.Data;
using System.Web;
using Microsoft.Data.SqlClient;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Dapper;
using System.Data.OracleClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Webapi.interfaces;
using Webapi.InversionOfControl;
using Webapi.enums;

namespace Webapi.services
{
    [RegisterIOC]
    public class MembersDBServices : IMembersDBServices
    {
        private OpenposeContext _openposeContext { get; set; }
        public MembersDBServices(OpenposeContext openposeContext)
        {
            _openposeContext = openposeContext;
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="Data">會員資料</param>
        /// <param name="image">大頭貼</param>
        public async Task register(Members Data, string image)
        {
            Data.password = await HashPassword(Data.password);
            ImageFile ImageData = new ImageFile();
            ImageData.ImageFileId = Data.ImageFileId;
            ImageData.image = image;
            await _openposeContext.ImageFile.AddAsync(ImageData);
            await _openposeContext.Members.AddAsync(Data);
            await _openposeContext.SaveChangesAsync();
        }
        /// <summary>
        /// 雜湊密碼
        /// </summary>
        /// <param name="password">密碼</param>
        public async Task<string> HashPassword(string password)
        {
            string salt = "ojigojidfgjoisrgfjio";
            string sultAndPassword = string.Concat(salt, password);
            SHA256CryptoServiceProvider sHA256 = new SHA256CryptoServiceProvider();
            byte[] Change = Encoding.UTF8.GetBytes(sultAndPassword);
            byte[] Hash = sHA256.ComputeHash(Change);
            string HashPassword = Convert.ToBase64String(Hash);
            await Task.Delay(1);
            return HashPassword;
        }
        /// <summary>
        /// 用MembersId查詢一筆資料
        /// </summary>
        /// <param name="MembersId">帳號</param>
        public async Task<Members> GetDataByMembersId(string MembersId)
        {
            var Data = await _openposeContext.Members.FindAsync(MembersId);
            return Data;
        }
        /// <summary>
        /// 用ImageFileId查詢一筆資料
        /// </summary>
        /// <param name="ImageFileId">用Id找圖片路徑</param>
        public async Task<ImageFile> GetDataByImageId(string ImageFileId)
        {
            var Data = await _openposeContext.ImageFile.FindAsync(ImageFileId);
            return Data;
        }
        /// <summary>
        /// 檢查帳號是否重複
        /// </summary>
        /// <param name="MembersId">帳號</param>
        public async Task<bool> MembersIdCheck(string MembersId)
        {
            Members Data = await GetDataByMembersId(MembersId);
            return (Data == null);
        }
        /// <summary>
        /// 信箱驗證
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="authcode">驗證碼</param>
        public async Task<string> Validate(string MembersId, string authcode)
        {
            Members Data = await GetDataByMembersId(MembersId);
            if (Data != null)
            {
                if (Data.authcode == authcode)
                {
                    var update = await _openposeContext.Members.FindAsync(MembersId);
                    update.authcode = string.Empty;
                    _openposeContext.SaveChanges();
                    return "註冊成功";
                }
                else
                {
                    return "資料傳輸錯誤請重新註冊";
                }
            }
            else
            {
                return "帳號尚未註冊請去註冊";
            }
        }
        /// <summary>
        /// 計算當天有幾個帳號
        /// </summary>
        public async Task<int> CountMembers()
        {
            var result = await _openposeContext.Members.ToListAsync();
            var count = result.Count(a => a.ImageFileId.StartsWith(DateTime.Now.ToString("yyyyMMdd")));
            return count;
        }
        /// <summary>
        /// 流水號產生
        /// </summary>
        /// <param name="ID">流水號</param>
        public async Task<string> MakeStrId(int ID)
        {
            string StrID = ID.ToString().PadLeft(7, '0');
            await Task.Delay(1);
            return StrID;
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="MembersId">帳號</param>
        /// <param name="password">密碼</param>
        public async Task<string> login(string MembersId, string password)
        {
            Members Data = await GetDataByMembersId(MembersId);
            if (Data != null)
            {
                if (!Data.DeleteOrNo)
                {
                    if (string.IsNullOrWhiteSpace(Data.authcode))
                    {
                        if (await PasswordCheck(Data, password))
                        {
                            return "";
                        }
                        else
                        {
                            return "帳號或密碼錯誤";
                        }
                    }
                    else
                    {
                        return "帳號尚未驗證";
                    }
                }
                else
                {
                    return "該帳號尚未註冊";
                }
            }
            else
            {
                return "該帳號尚未註冊";
            }
        }
        /// <summary>
        /// 密碼確認
        /// </summary>
        /// <param name="Data">會員資料</param>
        /// <param name="Password">密碼</param>
        public async Task<bool> PasswordCheck(Members Data, string Password)
        {
            return (Data.password == await HashPassword(Password));
        }
        /// <summary>
        /// 取得角色(管理員)
        /// </summary>
        /// <param name="MembersId">帳號</param>
        public async Task<string> GetRole(string MembersId)
        {
            string Role = "User";
            Members Data = await GetDataByMembersId(MembersId);
            if (Data.isadmin)
            {
                Role += ",Admin";
            }
            return Role;
        }
        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="MembersId">會員帳號</param>
        /// <param name="newpassword">新的密碼</param>
        public async Task<string> changepassword(string MembersId, string newpassword)
        {
            Members changeData = await GetDataByMembersId(MembersId);
            var Data = await _openposeContext.Members.FindAsync(MembersId);
            Data.password = await HashPassword(newpassword);
            _openposeContext.SaveChanges();
            return "修改成功";
        }
        /// <summary>
        /// 頭貼
        /// </summary>
        /// <param name="MembersId">會員帳號</param>
        /// <param name="image">更換頭貼的新路徑</param>
        public async Task<string> changeheadshot(string MembersId, string image)
        {
            Members changeData = await GetDataByMembersId(MembersId);
            var Data = await _openposeContext.ImageFile.FindAsync(changeData.ImageFileId);
            Data.image = image;
            _openposeContext.SaveChanges();
            return "上傳大頭貼成功";
        }
        /// <summary>
        /// 取得帳號用的ImagefileId
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        /// <returns>ImagefileId</returns>
        public async Task<string> GetMemberCreateImagefileId(string MembersId)
        {
            string ImagefileId = string.Empty;
            string Date = DateTime.Now.ToString("yyyyMMdd") + DataIdType.H;
            var Db = await _openposeContext.ImageFile.ToListAsync();
            int newimagefileIdnum = 0;
            var newimagefileId = Db.OrderByDescending(x => x.ImageFileId)
                                    .FirstOrDefault(x => x.ImageFileId.Contains(Date)); //尋找最新一筆的ImagefileId
            if (newimagefileId != null)
            {
                string[] newimagefileIdsplit = newimagefileId.ImageFileId.ToString().Split("H"); //把ImagefileId用H隔開
                newimagefileIdnum = Convert.ToInt32(newimagefileIdsplit[1]); //陣列元素的第一個就是ImagefileId命名到底幾個
            }
            ImagefileId = DateTime.Now.ToString("yyyyMMdd") + DataIdType.H + string.Format("{0:0000000}", newimagefileIdnum + 1);
            return ImagefileId;
        }
        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        public async Task DeleteMemberData(string MembersId)
        {
            var MembersData = await _openposeContext.Members.FindAsync(MembersId);
            if (MembersData != null)
            {
                MembersData.DeleteOrNo = true;
            }
            _openposeContext.SaveChanges();
        }
        /// <summary>
        /// 恢復帳號
        /// </summary>
        /// <param name="MembersId">帳號Id</param>
        public async Task RestoreMembers(string MembersId, string password , string authcode)
        {
            var MembersData = await _openposeContext.Members.FindAsync(MembersId);
            if (MembersData != null)
            {
                MembersData.password = await HashPassword(password);
                MembersData.authcode = authcode;
                MembersData.DeleteOrNo = false;
            }
            _openposeContext.SaveChanges();
        }
    }
}