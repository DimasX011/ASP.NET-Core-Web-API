using MetricsManager.Response;
using MetricsManager.AllRequestMetric;
using MetricsManager.Client;
using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotnetMetricsController : ControllerBase
    {
        private readonly ILogger<DotnetMetricsController> _logger;
        private HttpClient _httpClient;
        private IDotnetInterfaceRepository repository;
        private readonly IMapper mapper;

        public DotnetMetricsController(ILogger<DotnetMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            this.mapper = mapper;
            _logger.LogDebug(1, "NLog встроен в DotnetMetricsController");
        }


        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgentCpu([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            // логируем, что мы пошли в соседний сервис
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис
            MetricsAgentClient metrics = new MetricsAgentClient(_httpClient, _logger);
            metrics.GetDotNetMetrics(new GetAllDotnetMetricsApiRequest
            {
                FromTime = fromTime,
                ToTime = toTime,

            });

            // возвращаем ответ
            return Ok(metrics);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetAllMetric(DateTime toTime)
        {
            var metrics = repository.GetAllMetrics(toTime);
            var response = new AllDotnetMetricResponse()
            {
                Metrics = new List<DotnetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<DotnetMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<DotnetMetric> metrics = repository.GetAll();

            var response = new AllDotnetMetricResponse()
            {
                Metrics = new List<DotnetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<DotnetMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AllCreateRequest request)
        {
            _logger.LogInformation("запрос", request);

            repository.Create(new DotnetMetric
            {
                Time = request.Time,
                Value = request.Value

            });


            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

   
    }
}
