using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MetricsManager.Client.ApiRequest;
using MetricsManager.Client;
using MetricsManager.DAL.Repositories;
using MetricsManager.AllRequestMetric;
using MetricsManager.DAL.Models;
using MetricsManager.Response;
using AutoMapper;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentMetricsController : ControllerBase
    {
        private readonly ILogger<AgentMetricsController> _logger;
        private readonly IHttpClientFactory clientFactory;
        private IAgentIterfaceRepository repository;
        private readonly IMapper mapper;

        public AgentMetricsController(ILogger<AgentMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            this.mapper = mapper; 
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AllCreateRequest request)
        {
            _logger.LogInformation("запрос", request);
           
            repository.Create(new AgentMetric 
            {
                Time = request.Time,
                Value = request.Value

                
            });
            
         
            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"http://localhost:50343/api/cpumetrics/from/1/to/999999?var=val&var1=val1");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            var client = clientFactory.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                var metricsResponse = JsonSerializer.DeserializeAsync
                <AllCpuMetricsResponse>(responseStream).Result;
            }
            else
            {
                // ошибка при получении ответа
            }
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetAllMetric(DateTime toTime)
        {
            var metrics = repository.GetAllMetrics(toTime);
            var response = new AllAgentMetricResponse()
            {
                Metrics = new List<AgentMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<AgentMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<AgentMetric> metrics = repository.GetAll();

            var response = new AllAgentMetricResponse()
            {
                Metrics = new List<AgentMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<AgentMetricDto>(metric));
            }

            return Ok(response);
        }



        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogDebug("Регистрация пользователя:", agentInfo);
            return Ok();
           
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogDebug("Регистрация пользователя:", agentId);
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogDebug("Регистрация пользователя:", agentId);
            return Ok();
        }

    }
}
