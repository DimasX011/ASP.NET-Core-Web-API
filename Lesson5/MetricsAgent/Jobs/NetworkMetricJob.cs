using MetricsAgent.Dal.Models;
using MetricsAgent.Dal.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob
    {
        private INetworkMetricRepoitory _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public NetworkMetricJob(INetworkMetricRepoitory repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Network usage", "% Network load time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var NetworkUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time =
            TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new NetworkMetric
            {
                Time = time,
                Value =
            NetworkUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
