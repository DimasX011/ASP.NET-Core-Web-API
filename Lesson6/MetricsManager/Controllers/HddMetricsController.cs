using AutoMapper;
using MetricsManager.AllRequestMetric;
using MetricsManager.Client;
using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private HttpClient _httpClient;
        private IhddMetricInterface repository;
        private readonly IMapper mapper;


        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgentCpu([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime, IMapper mapper)
        {
            // логируем, что мы пошли в соседний сервис
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис
            MetricsAgentClient metrics = new MetricsAgentClient(_httpClient, _logger);
            metrics.GetAllHddMetrics(new GetAllHddMetricsApiRequest
            {
                FromTime = fromTime,
                ToTime = toTime,

            });

            // возвращаем ответ
            return Ok(metrics);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AllCreateRequest request)
        {
            _logger.LogInformation("запрос", request);

            repository.Create(new HddMetric
            {
                Time = request.Time,
                Value = request.Value

            });


            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<HddMetric> metrics = repository.GetAll();

            var response = new AllHddMetricResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetAllMetric(DateTime toTime)
        {
            var metrics = repository.GetAllMetrics(toTime);
            var response = new AllHddMetricResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));
            }
            return Ok(response);
        }

        public HddMetricsController(ILogger<HddMetricsController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }
    }
}
