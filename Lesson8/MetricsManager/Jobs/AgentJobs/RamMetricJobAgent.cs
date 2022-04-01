using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace MetricsManager.Jobs.AgentJobs
{
    public class RamMetricJobAgent : IJob
    {
        private IAgentIterfaceRepository _repository;
        // Счётчик для метрики CPU

        public RamMetricJobAgent(IAgentIterfaceRepository repository)
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