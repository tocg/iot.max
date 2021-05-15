/*
 * Swagger是.net5.0自带，如果手动，则安装包
 * Swashbuckle.AspNetCore
 * 
 * Log4Net安装包：
 * Microsoft.Extensions.Logging.Log4Net.AspNetCore
 * 
 * Cors安装包：
 * Microsoft.AspNetCore.Cors
 * 
 * **/

using AutoMapper;
using Iot.Max.Api.Controllers.WS;
using Iot.Max.Lib;
using Iot.Max.Model.Models.Token;
using Iot.Max.Services;
using Iot.Max.Services.Document;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Api
{
    public class Startup
    {

        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            repository = LogManager.CreateRepository("rollingAppender");
            XmlConfigurator.Configure(repository, new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));

        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //添加对AutoMapper的支持
            services.AddAutoMapper(typeof(AutomapperConfig));

            #region 自带Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iot.Max.Api", Version = "v1" });

                #region 为Swagger JSON and UI设置xml文档注释路径
                //（在这之前需要【项目】-【属性】-【生成】勾选xml文档）

                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = System.IO.Path.Combine(basePath, "Iot.Max.Api.xml");
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region 在swagger中添加授权认证的测试
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                #endregion
            });
            #endregion

            #region 跨域配置

            services.AddCors(p =>
            {
                p.AddPolicy("IotMaxCors", c =>
                {
                    c.AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowCredentials();
                });
            });
            #endregion

            #region 数据库操作


            //dapper的sqlserver
            //services.AddDapper("SqlDb", m =>
            //{
            //    m.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            //    m.DbType = DbStoreType.SqlServer;
            //});

            //dapper的mysql
            services.AddDapper(DbStoreType.MySql.ToString().ToLower(), m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("MySqlConnection");
                m.DbType = DbStoreType.MySql;
            });

            //ado.net
            //services.AddTransient<IDataAccess, SqlServerDataAccess>();

            #endregion

            #region 注册业务服务层
            //services.AddTransient<IServices>(x =>  new BaseServices(new DapperClientHelper(Configuration)) );
            services.AddTransient<IServices, BaseServices>();
            services.AddTransient<DocumentOrderServices>();
            #endregion

            #region 默认日期带T格式（默认ISO-8601格式造成的）

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullableConverter());
            });
            #endregion

            #region JWT验证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;  //是否必须用https协议
                option.SaveToken = true;  //生成token,是否保存到上下文中，并向后传递

                //生成token的一些参数验证，都从配置文件中获取
                var token = Configuration.GetSection("tokenParameter").Get<TokenParameter>();
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
            });
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iot.Max.Api v1"));
            }


            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),                
                ReceiveBufferSize = 1 * 1024
            });
            app.UseMiddleware<WebsocketHandlerMiddleware>();

            app.UseStaticFiles(); //使用上传的图片

            app.UseRouting();

            app.UseCors("IotMaxCors");

            app.UseAuthentication(); //启用身份验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
