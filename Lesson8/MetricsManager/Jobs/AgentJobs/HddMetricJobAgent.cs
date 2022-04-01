using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs.AgentJobs
{
    public class HddMetricJobAgent
    {
        private IhddMetricInterface _repository;
        // Счётчик для метрики CPU

        public HddMetricJobAgent(IhddMetricInterface repository)
        {
            _repository = repository;
        }
 

        public Task Execute (IJobExecutionContext context)
        {
            _repository.GetLastTime();
            return Task.CompletedTask;
        }
    }
}
