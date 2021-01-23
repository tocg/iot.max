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
using Iot.Max.Lib;
using Iot.Max.Services;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //业务服务层
            //services.AddTransient<IServices>(x =>  new BaseServices(new DapperClientHelper(Configuration)) );
            services.AddTransient<IServices, BaseServices>();

            #region 默认日期带T格式（默认ISO-8601格式造成的）

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullableConverter());
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iot.Max.Api v1"));
            }

            app.UseStaticFiles(); //使用上传的图片

            app.UseRouting();

            app.UseCors("IotMaxCors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
