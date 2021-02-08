using fmptV2.Common.Cache;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using fmptV2.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fmptV2.Controllers
{
    [VLAuthentication]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("AllCors")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var rng = new Random();
            Log4NetLogger.Info(rng.ToString());
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("AllCors")]
        public string TestRedis([FromServices] RedisCache cache)
        {
            cache.Set<string>("111", "111", DateTime.Now.AddMinutes(30));
            var value = cache.Get<string>("111");
            return value;
        }
    }
}
