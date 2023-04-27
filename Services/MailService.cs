using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Threading.Tasks;
using Webapi.interfaces;
using Webapi.InversionOfControl;

namespace Webapi.services
{
    [RegisterIOC]
    public class MailService : IMailServices
    {
        private string Gmail_Account = "apple171800";
        private string Gmail_Password = "dkplkspuatprocbq";
        private string Gmail_mail = "apple171800@gmail.com";

        /// <summary>
        /// 製作驗證碼
        /// </summary>
        public async Task<string> makeAuthCode()
        {
            string[] Code = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                "0","1","2","3","4","5","6","7","8","9"};
            string AuthCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                AuthCode += Code[rd.Next(Code.Count())];
            }
           await Task.Delay(1);
            return  AuthCode;
        }
        /// <summary>
        /// 驗證信件內容
        /// </summary>
        /// <param name="Temp">信件範本</param>
        /// <param name="UserName">使用者名稱</param>
        /// <param name="ValidataUrl">驗證網址</param>
        public  async Task<string> Getmailbody(string Temp, string UserName, string ValidataUrl)
        {
            Temp = Temp.Replace("{{UserName}}", UserName);
            Temp = Temp.Replace("{{ValidataUrl}}", ValidataUrl);
            await Task.Delay(1);
            return Temp;
        }
        /// <summary>
        /// 寄信方法
        /// </summary>
        /// <param name="mailbody">信件本體</param>
        /// <param name="ToMail">要寄送的信箱</param>
        public async Task sendMail(string mailbody, string ToMail,string MailTitle)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(Gmail_Account, Gmail_Password);
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.To.Add(ToMail);
            msg.From = new MailAddress(Gmail_mail);
            msg.Subject = MailTitle;
            msg.Body = mailbody;
            msg.IsBodyHtml = true;
            client.Send(msg);
            await Task.Delay(1);
        }
    }
}