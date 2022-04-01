using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs.BasicJobs
{
    public class HddMetricJob
    {
        private IhddMetricInterface _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public HddMetricJob(IhddMetricInterface repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Hdd usage", "% Hdd usage Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var HddUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time =
            TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new HddMetric
            {
                Time = time,
                Value =
            HddUsageInPercents
            });
            return Task.CompletedTask;
        }

        public Task ExecuteClient (IJobExecutionContext context)
        {
            var HddUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.GetLastTime();
            return Task.CompletedTask;
        }
    }
}
