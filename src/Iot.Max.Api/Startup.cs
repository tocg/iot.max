/*
 * Swagger��.net5.0�Դ�������ֶ�����װ��
 * Swashbuckle.AspNetCore
 * 
 * Log4Net��װ����
 * Microsoft.Extensions.Logging.Log4Net.AspNetCore
 * 
 * Cors��װ����
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

            //��Ӷ�AutoMapper��֧��
            services.AddAutoMapper(typeof(AutomapperConfig));

            #region �Դ�Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iot.Max.Api", Version = "v1" });

                #region ΪSwagger JSON and UI����xml�ĵ�ע��·��
                //������֮ǰ��Ҫ����Ŀ��-�����ԡ�-�����ɡ���ѡxml�ĵ���

                //��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = System.IO.Path.Combine(basePath, "Iot.Max.Api.xml");
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region ��swagger�������Ȩ��֤�Ĳ���
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

            #region ��������

            services.AddCors(p =>
            {
                p.AddPolicy("IotMaxCors", c =>
                {
                    c.AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowCredentials();
                });
            });
            #endregion

            #region ���ݿ����


            //dapper��sqlserver
            //services.AddDapper("SqlDb", m =>
            //{
            //    m.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            //    m.DbType = DbStoreType.SqlServer;
            //});

            //dapper��mysql
            services.AddDapper(DbStoreType.MySql.ToString().ToLower(), m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("MySqlConnection");
                m.DbType = DbStoreType.MySql;
            });

            //ado.net
            //services.AddTransient<IDataAccess, SqlServerDataAccess>();

            #endregion

            #region ע��ҵ������
            //services.AddTransient<IServices>(x =>  new BaseServices(new DapperClientHelper(Configuration)) );
            services.AddTransient<IServices, BaseServices>();
            services.AddTransient<DocumentOrderServices>();
            #endregion

            #region Ĭ�����ڴ�T��ʽ��Ĭ��ISO-8601��ʽ��ɵģ�

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullableConverter());
            });
            #endregion

            #region JWT��֤
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;  //�Ƿ������httpsЭ��
                option.SaveToken = true;  //����token,�Ƿ񱣴浽�������У�����󴫵�

                //����token��һЩ������֤�����������ļ��л�ȡ
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

            app.UseStaticFiles(); //ʹ���ϴ���ͼƬ

            app.UseRouting();

            app.UseCors("IotMaxCors");

            app.UseAuthentication(); //���������֤
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
