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
    public class DotnetMetricJob
    {
        private IDotnetInterfaceRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;

        public DotnetMetricJob(IDotnetInterfaceRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Dotnet ", "% Dotnet Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var DotnetUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time =
            TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new DotnetMetric
            {
                Time = time,
                Value =
            DotnetUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
