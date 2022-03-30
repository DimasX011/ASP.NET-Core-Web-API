using AutoMapper;
using MetricsAgent.Response;
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

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private HttpClient _httpClient;
        private IRamMetricRepository repository;
        private IMapper mapper;

        public RamMetricsController(ILogger<RamMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AllCreateRequest request)
        {
            _logger.LogInformation("запрос", request);

            repository.Create(new RamMetric
            {
                Time = request.Time,
                Value = request.Value

            });


            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgentNetwork([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            // логируем, что мы пошли в соседний сервис
            _logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис
            MetricsAgentClient metrics = new MetricsAgentClient(_httpClient, _logger);
            metrics.GetAllRamMetrics(new GetAllRamMetricsApiRequest
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
            var response = new AllRamMetricResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<RamMetric> metrics = repository.GetAll();

            var response = new AllRamMetricResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
