using MetricsAgent.Dal.Models;
using MetricsAgent.DAL.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
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
    }
}
