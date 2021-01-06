using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Routine.Date;
using Routine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace Routine
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
            services.AddControllers(setup => 
            {
                setup.ReturnHttpNotAcceptable = true;  //请求的application format不符合要求返回406状态码
                //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());  //输出xml格式的数据，旧方法
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter()); //将xml输出格式改为默认选项
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                setup.Filters.Add(new AuthorizeFilter(policy)); //配置授权
            }).AddXmlDataContractSerializerFormatters() //配置输入输出均支持XML
                .AddNewtonsoftJson(setup=>setup.SerializerSettings.ContractResolver=
                    new CamelCasePropertyNamesContractResolver()); //安装依赖项Microsoft.AspNetCore.Mvc.NewtonsoftJson，配置命名规范

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //AutoMapper配置
            services.AddDbContext<RoutineIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RoutineIdentityDatabase")));//身份认证数据库
            services.AddDbContext<RoutineDbContext>(optionsAction: options => options.UseSqlServer(Configuration.GetConnectionString("RoutineDatabase"))); //连接数据库
            services.AddScoped<ICompanyRepository, CompanyRepository>(); //模型依赖注入
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RoutineIdentityDbContext>();
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredUniqueChars = 3;
            //    options.Password.RequiredLength = 6;
            //});
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001/";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected Error");
                    }));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
