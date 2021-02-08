using fmptV2.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace fmptV2
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                //Json≈‰÷√
                builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                builder.AddJsonFile("configs/config.json", optional: false, reloadOnChange: false);
                //æ≤Ã¨≥£¡ø
                var configs = builder.Build();
                APIContraints.DBConfig = configs.GetSection("DB").Get<DBConfig>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
