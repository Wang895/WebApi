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
                setup.ReturnHttpNotAcceptable = true;  //�����application format������Ҫ�󷵻�406״̬��
                //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());  //���xml��ʽ�����ݣ��ɷ���
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter()); //��xml�����ʽ��ΪĬ��ѡ��
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                setup.Filters.Add(new AuthorizeFilter(policy)); //������Ȩ
            }).AddXmlDataContractSerializerFormatters() //�������������֧��XML
                .AddNewtonsoftJson(setup=>setup.SerializerSettings.ContractResolver=
                    new CamelCasePropertyNamesContractResolver()); //��װ������Microsoft.AspNetCore.Mvc.NewtonsoftJson�����������淶

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //AutoMapper����
            services.AddDbContext<RoutineIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RoutineIdentityDatabase")));//�����֤���ݿ�
            services.AddDbContext<RoutineDbContext>(optionsAction: options => options.UseSqlServer(Configuration.GetConnectionString("RoutineDatabase"))); //�������ݿ�
            services.AddScoped<ICompanyRepository, CompanyRepository>(); //ģ������ע��
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
