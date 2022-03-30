using MetricsManager.Client;
using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class AgentMetricJob
    {
        private IAgentIterfaceRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;
        private IMetricsAgentClient client;

        public AgentMetricJob(IAgentIterfaceRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Agent", "% Agent usage Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var AgentUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time =
            TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new AgentMetric
            {
                Time = time,
                Value =
            AgentUsageInPercents
            });
            return Task.CompletedTask;
        }

        public Task ExecuteCl(IJobExecutionContext context)
        {

            return Task.CompletedTask;
        }



    }
}
