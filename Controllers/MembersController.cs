using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webapi.services;
using Webapi.Models;
using Webapi.Request;
using Webapi.Response;
using System.IO;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Webapi.interfaces;
using Microsoft.AspNetCore.Hosting;
using Webapi.Filters;
using System.Drawing;
using System.Drawing.Imaging;

namespace Webapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MembersController : ControllerBase
    {
        private readonly OpenposeContext _openposeContext;
        private readonly IMembersDBServices _membersDBServices;
        private readonly IMailServices _mailService;
        private readonly IConfiguration _configuration;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembersController
        (
            IMembersDBServices membersDBService,
            IMailServices mailService,
            OpenposeContext openposeContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment WebHostEnvironment
        )
        {
            _membersDBServices = membersDBService;
            _mailService = mailService;
            _openposeContext = openposeContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _WebHostEnvironment = WebHostEnvironment;
        }
        /// <summary>
        /// 信箱驗證
        /// </summary>
        /// <param name="MembersId">註冊帳號</param>
        /// <param name="authcode">驗證碼</param>
        [HttpGet]
        //不受驗證影響
        [AllowAnonymous]
        public async Task<IActionResult> EmailValidate(string MembersId, string authcode)
        {
            string result = await _membersDBServices.Validate(MembersId, authcode);
            return Ok(new ApiResponse
            {
                Status = 200,
                Data = result
            });
        }
        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="registerMember">註冊資料</param>
        [HttpPost]
        //不受驗證影響
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(StartandEndRequest ForgetPasswordData)
        {
            Members Data = await _membersDBServices.GetDataByMembersId(ForgetPasswordData.MembersId);
            if (Data != null)
            {
                if (!Data.DeleteOrNo)
                {
                    if (string.IsNullOrEmpty(Data.authcode))
                    {
                        string newpassword = await _mailService.makeAuthCode();
                        await _membersDBServices.changepassword(ForgetPasswordData.MembersId, newpassword);
                        string TempMail = System.IO.File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory + "\\wwwroot\\ForgetPaswordEmailTemplate.html"));
                        string MailBody = await _mailService.Getmailbody(TempMail, Data.name, newpassword);
                        await _mailService.sendMail(MailBody, Data.MembersId, "修改密碼通知信");
                        return Ok(new ApiResponse
                        {
                            Status = 200,
                            Data = "重製完成請去信箱收取最新密碼"
                        });
                    }
                    else
                    {
                        return Ok(new ApiResponse
                        {
                            Status = 404,
                            ErrorMessage = "該帳號尚未進行信箱驗證"
                        });
                    }
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Status = 404,
                        ErrorMessage = "該帳號尚未註冊"
                    });
                }
            }
            else
            {
                return Ok(new ApiResponse
                {
                    Status = 404,
                    ErrorMessage = "該帳號尚未註冊"
                });
            }
        }
        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="RegisterMember">註冊資料</param>
        [HttpPost]
        //不受驗證影響
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest RegisterMember)
        {
            if (ModelState.IsValid)
            {
                if (RegisterMember.Password == RegisterMember.Passwordcheck)
                {
                    if (await _membersDBServices.MembersIdCheck(RegisterMember.MembersId))
                    {
                        Members Data = new Members();
                        string ImageUrl = string.Empty;
                        //取得當前資料夾路徑
                        string content_path = _WebHostEnvironment.ContentRootPath;
                        //提供預設圖片
                        ImageUrl = Path.Combine(content_path + @"\wwwroot\Image\defult.jpg");
                        //設立ImageFileId
                        Data.ImageFileId = await _membersDBServices.GetMemberCreateImagefileId(RegisterMember.MembersId);
                        Data.MembersId = RegisterMember.MembersId;
                        string[] name = RegisterMember.MembersId.Split('@');
                        Data.name = name[0];
                        Data.password = RegisterMember.Password;
                        Data.authcode = await _mailService.makeAuthCode();
                        await _membersDBServices.register(Data, ImageUrl);
                        string TempMail = System.IO.File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory + "\\wwwroot\\RegisterEmailTemplate.html"));
                        string ValidateUrl = @$"https://localhost:5001/Index.html?MembersId={RegisterMember.MembersId},authcode={Data.authcode}";
                        string MailBody = await _mailService.Getmailbody(TempMail,
                            Data.name,
                            ValidateUrl.ToString().Replace("%3F", "?"));
                        await _mailService.sendMail(MailBody, Data.MembersId, "會員註冊驗證信");
                        return Ok(new ApiResponse
                        {
                            Status = Response.StatusCode = 200,
                            Data = "註冊成功，請到信箱收信",
                        });
                    }
                    else
                    {
                        Members membersData = await _membersDBServices.GetDataByMembersId(RegisterMember.MembersId);
                        if (membersData.DeleteOrNo)
                        {
                            string authcode = await _mailService.makeAuthCode();
                            await _membersDBServices.RestoreMembers(RegisterMember.MembersId, RegisterMember.Password, authcode);
                            string TempMail = System.IO.File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory + "\\wwwroot\\RegisterEmailTemplate.html"));
                            string ValidateUrl = @$"https://localhost:5001/Index.html?MembersId={RegisterMember.MembersId},authcode={authcode}";
                            string MailBody = await _mailService.Getmailbody(TempMail,
                                membersData.name,
                                ValidateUrl.ToString().Replace("%3F", "?"));
                            await _mailService.sendMail(MailBody, RegisterMember.MembersId, "會員註冊恢復帳號驗證信");
                            return Ok(new ApiResponse
                            {
                                Status = 200,
                                Data = "註冊成功，請到信箱收信"
                            });
                        }
                        else
                        {
                            return Ok(new ApiResponse
                            {
                                Status = 404,
                                ErrorMessage = "該帳號已被註冊"
                            });
                        }
                    }
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Status = 404,
                        ErrorMessage = "確認密碼不正確"
                    });
                }
            }
            else
            {
                return Ok(new ApiResponse
                {
                    Status = 404,
                    ErrorMessage = "這不是信箱的格式"
                });
            }
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="LoginData">登入資料</param>
        [HttpPost]
        //不受驗證影響
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest LoginData)
        {
            string ValidataStr = await _membersDBServices.login(LoginData.MembersId, LoginData.Password);
            Members user = await _membersDBServices.GetDataByMembersId(LoginData.MembersId);
            if (user != null)
            {
                if (!user.DeleteOrNo)
                {
                    ImageFile userImage = await _membersDBServices.GetDataByImageId(user.ImageFileId);
                    string[] imageFileUrl = userImage.image.Split(@"\");
                    int a = imageFileUrl.Length;
                    if (string.IsNullOrEmpty(ValidataStr))
                    {
                        string Role = await _membersDBServices.GetRole(LoginData.MembersId);
                        string[] Roles = Role.Split(new char[] { ',' });
                        var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId,user.name),
                    new Claim(JwtRegisteredClaimNames.Email,user.MembersId),
                };
                        foreach (var temp in Roles)
                        {
                            claims.Add(new Claim("role", temp));
                        }
                        //取出appsettings.json裡的KEY處理
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                        //設定jwt相關資訊
                        var jwt = new JwtSecurityToken
                        (
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Issuer"],
                            claims: claims,
                            expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JWT:ExprieDay"])),
                            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                        );
                        //產生JWT Token
                        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                        return Ok(new LoginResponse
                        {
                            Status = 200,
                            Data = "登入成功",
                            Token = token,
                            MembersId = user.MembersId,
                            Name = user.name,
                            ImageUrl = imageFileUrl[a - 2] + @"\" + imageFileUrl[a - 1]
                        });
                    }

                    else
                    {
                        return Ok(new LoginResponse
                        {
                            Status = 404,
                            ErrorMessage = ValidataStr
                        });
                    }
                }
                else
                {
                    return Ok(new LoginResponse
                    {
                        Status = 404,
                        ErrorMessage = ValidataStr
                    });
                }
            }
            else
            {
                return Ok(new LoginResponse
                {
                    Status = 404,
                    ErrorMessage = ValidataStr
                });
            }
        }
        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="ChangeData">修改密碼資料</param>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangeRequest ChangeData)
        {
            var MembersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Members Data = await _membersDBServices.GetDataByMembersId(MembersId);
            if (Data.password == await _membersDBServices.HashPassword(ChangeData.Password))
            {
                string ChangeStr = await _membersDBServices.changepassword(MembersId, ChangeData.Newpassword);
                return Ok(new ApiResponse
                {
                    Status = 200,
                    Data = ChangeStr
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    Status = 404,
                    ErrorMessage = "舊密碼錯誤"
                });
            }
        }
        /// <summary>
        /// 更換頭貼
        /// </summary>
        /// <param name="headshot_DataUrl">頭貼base64</param>
        [HttpPost]
        public async Task<IActionResult> Changephoto(ChangeheadshotRequest headshot_DataUrl)
        {
            //取得使用者資訊
            var MembersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            //設定檔案儲存位置
            var content_path = _WebHostEnvironment.ContentRootPath + @"\wwwroot\Image\";
            string headshot_base64 = headshot_DataUrl.Headshot_Base64.Substring(headshot_DataUrl.Headshot_Base64.IndexOf(',') + 1);
            byte[] headshot_byte = Convert.FromBase64String(headshot_base64);
            string imagefile = content_path + MembersId + ".jpg";
            string result = await _membersDBServices.changeheadshot(MembersId, imagefile);
            using (Stream headshot_stream = new MemoryStream(headshot_byte))
            {
                Image img = Image.FromStream(headshot_stream);
                img.Save(imagefile, ImageFormat.Jpeg);
            }
            string[] imageFileUrl = imagefile.Split(@"\");
            int a = imageFileUrl.Length;
            await Task.Delay(1);
            return Ok(new LoginResponse
            {
                Status = 200,
                Data = result,
                ImageUrl = imageFileUrl[a - 2] + @"\" + imageFileUrl[a - 1]
            });
        }
        /// <summary>
        /// 刪除帳號
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteMembers()
        {
            var MembersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            await _membersDBServices.DeleteMemberData(MembersId);
            return Ok(new ApiResponse
            {
                Status = 200,
                Data = "刪除帳號成功"
            });
        }
    }
}
