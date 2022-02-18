using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherL1;

namespace weatherL1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        public List<WeatherForecast> weathers { get; set; }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


        private readonly ILogger<WeatherForecastController> _logger;

        private readonly WeatherHolder weather;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherHolder weathers)
        {
            _logger = logger;
            weather = weathers;
        }


        [HttpPost("save")]
        public IActionResult Create([FromQuery] int tempF, string date)
        {
            weather.Add(tempF, date);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult Read()
        {
            return Ok(weather.Get());
        }

        [HttpPut("update")]
        public IActionResult Update([FromQuery] int newtemp, [FromQuery] string newdateTime, [FromQuery] int temp, [FromQuery] string dateTime)
        {
            for (int i = 0; i < weather.weathers.Count; i++)
            {
                if (weather.weathers[i].TemperatureC == temp && weather.weathers[i].Date == dateTime)
                {
                    weather.weathers[i].Date = newdateTime;
                    weather.weathers[i].TemperatureC = newtemp;
                }
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int tempdel, [FromQuery] string dateTime)
        {
            for (int i = 0; i < weather.weathers.Count; i++)
            {
                if (weather.weathers[i].Date == dateTime && weather.weathers[i].TemperatureC == tempdel)
                {
                    weather.weathers.Remove(weather.weathers[i]);
                }
            }
            return Ok();
        }
    }
}