using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;

namespace Webapi.interfaces
{
    public interface IMailServices
    {
        /// <summary>
        /// 製作驗證碼
        /// </summary>
        Task<string> makeAuthCode();
        /// <summary>
        /// 信件內容
        /// </summary>
        /// <param name="Temp">信件範本</param>
        /// <param name="UserName">使用者名稱</param>
        /// <param name="ValidataUrl">驗證網址</param>
        Task<string> Getmailbody(string Temp, string UserName, string ValidataUrl);
        /// <summary>
        /// 寄信方法
        /// </summary>
        /// <param name="mailbody">信件本體</param>
        /// <param name="ToMail">要寄送的信箱</param>

        Task sendMail(string mailbody, string ToMail,string MailTitle);
    }
}