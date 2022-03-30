using AutoMapper;
using MetricsAgent.AgentMetricRepo;
using MetricsAgent.Dal.Models;
using MetricsAgent.Dal.Interfaces;
using MetricsAgent.Dal.Repositories;
using MetricsAgent.AllRequestMetric;
using MetricsAgent.Response;
namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<AgentMetric, AgentMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
        }
    }

}
