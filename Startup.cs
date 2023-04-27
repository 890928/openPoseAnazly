using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Webapi.services;
using Webapi.Models;
using Webapi.interfaces;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using Webapi.InversionOfControl;

namespace Webapi.Openpose
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApiDocument(); // 註冊服務加入 OpenAPI 文件

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi", Version = "v1" });
            });

            //註冊Cors
            services.AddCors(option => option.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            }));
            //資料庫連線
            services.AddDbContext<OpenposeContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("OpenposeContext"), new MySqlServerVersion(new Version(8, 0, 28))));
            //JWT Token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, //發行者驗證
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,//接收者驗證
                    ValidAudience = Configuration["Jwt:Issuer"],
                    ValidateLifetime = true, //存活時間驗證
                    ValidateIssuerSigningKey = true, //金鑰驗證
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            //Json
            services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }
            );
            //注入AutoMapper
            services.AddAutoMapper(typeof(Startup));
            //加入HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //所有功能加上權限
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });

            ConfigureInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OpenposeContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi();    // 啟動 OpenAPI 文件
            app.UseSwaggerUi3(); // 啟動 Swagger UI

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();
            //登入驗證
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //跨域
            app.UseCors("Policy");
            //建立資料庫
            dbContext.Database.EnsureCreated();
        }

        private static void ConfigureInjection(IServiceCollection services)
        {
            Type serviceExt = typeof(HttpClientFactoryServiceCollectionExtensions);
            MethodInfo method = serviceExt.GetMethod("AddHttpClient", 2, new Type[] { typeof(IServiceCollection) });
            var register = new RegusterIOC();
            register.DependencyInjection(
                (interfaceType, imType, life) =>
                {
                    switch (life)
                    {
                        case IocType.Scoped:
                            services.AddScoped(interfaceType, imType);
                            break;
                        case IocType.Singleton:
                            services.AddSingleton(interfaceType, imType);
                            break;
                        case IocType.Transient:
                            services.AddTransient(interfaceType, imType);
                            break;
                        default:
                            services.AddScoped(interfaceType, imType);
                            break;
                    }
                }, "Webapi"
            );
        }

    }
}
