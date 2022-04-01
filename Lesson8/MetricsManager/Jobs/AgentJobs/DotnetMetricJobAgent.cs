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
    public class DotnetMetricJobAgent
    {
        private IAgentIterfaceRepository _repository;
        // Счётчик для метрики CPU

        public DotnetMetricJobAgent(IAgentIterfaceRepository repository)
        {
            _repository = repository;
        }


        public Task Execute(IJobExecutionContext context)
        {
            _repository.GetLastTime();
            return Task.CompletedTask;
        }

    }
}
