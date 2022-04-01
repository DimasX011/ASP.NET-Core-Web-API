using MetricsAgent.Response;
using MetricsManager.Client.ApiRequest;

namespace MetricsManager.Client


{
    public interface IMetricsAgentClient
    {
        AllRamMetricResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request);
        AllHddMetricResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request);
        AllDotnetMetricResponse GetDotNetMetrics(DotNetHeapMetrisApiRequest request);
        AllCpuMetricsResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request);
        AllNetworkMetricResponse GetAllNetworkMetrics(GetAllNetworkApiRequest request);
        AllAgentMetricResponse GetAllAgentMetrics(GetAllAgentMetricsApiRequest request);





    }
}
