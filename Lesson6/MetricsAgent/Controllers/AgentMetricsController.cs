using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MetricsAgent.AllRequestMetric;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.Dal.Models;
using MetricsAgent.Dal.Interfaces;
using MetricsAgent.Response;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using AutoMapper;
using System;
using System.Net.Http;
using System.Text.Json;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentMetricsController : ControllerBase
    {
        private IAgentIterfaceRepository repository;
        private readonly ILogger<AgentMetricsController> _logger;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory clientFactory;
        public AgentMetricsController(IAgentIterfaceRepository repository, ILogger<AgentMetricsController> logger, IMapper mapper, IHttpClientFactory factory)
        {
            clientFactory = factory;
            this.mapper = mapper;
            this.repository = repository;
            _logger = logger;

        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AllCreateRequest request)
        {
            _logger.LogInformation("запрос",request);
            
            repository.Create(new AgentMetric
            {
                Time = request.Time,
                Value = request.Value
            });
            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

        [HttpGet("sql-read-write-test")]
        public IActionResult TryToInsertAndRead()
        {
            string connectionString = "Data Source=:memory:";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DROP TABLE IF EXISTS agentmetrics";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE agentmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(10,1)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(50,2)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(75,4)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(90,5)";
                    command.ExecuteNonQuery();
                    string readQuery = "SELECT * FROM agentmetrics LIMIT 3";
                    var returnArray = new GenerateData[3];
                    command.CommandText = readQuery;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        var counter = 0;
                        while (reader.Read())
                        {
                            returnArray[counter] = new GenerateData
                            {
                                Id = reader.GetInt32(0),
                                Value = reader.GetInt32(1),
                                Time = reader.GetInt64(2)
                            };

                            counter++;
                        }
                    }
                    return Ok(returnArray);
                }
            }
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
            _logger.LogInformation("Регистрация пользователя:", agentInfo);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult ReadAllRegistred()
        {
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Регистрация пользователя:", agentId);
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Регистрация пользователя:", agentId);
            return Ok();
        }
    }
}
