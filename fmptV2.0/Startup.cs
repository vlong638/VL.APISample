using fmptV2.Common;
using fmptV2.Common.Cache;
using fmptV2.Common.Filters;
using fmptV2.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace fmptV2
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
            //鉴权
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(GlobalExceptionFilter));
            }).AddJsonOptions(config =>
            {
                //此设定解决JsonResult中文被编码的问题
                config.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

                config.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                config.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
            });

            //HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Configs
            services.Configure<CORSConfig>(Configuration.GetSection("CORS"));
            services.Configure<DBConfig>(Configuration.GetSection("DB"));

            //Redis
            services.AddSingleton(p => new RedisCache(Configuration["Redis:ConnectionString"], Configuration["Redis:Prefix"]));
            APIContraints.RedisCache = new RedisCache(Configuration["Redis:ConnectionString"], Configuration["Redis:Prefix"]);

            //Context
            services.AddScoped<APIContext>();

            //Services
            services.AddScoped<AccountService, AccountService>();

            //CORS
            services.AddCors(option => option.AddPolicy("AllCors", policy => policy
                .WithOrigins(Configuration.GetSection("CORS").Get<CORSConfig>().Origins.Select(c => c.Value).ToArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            ));

            //服务接口管理
            services.AddSwaggerGen(p =>
            {
                p.OperationFilter<AuthHeaderFilter>();
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "fmptV2.0", Version = "v1" });
                p.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "fmptV2.0.xml");
                p.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //CORS
            app.UseCors("AllCors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "VL API");
            });
        }
    }
}
