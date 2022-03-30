using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Dal.Models;
using MetricsAgent.Response;

namespace MetricsAgent.AgentmetricRepo
{
    public interface IAgentInterface<T> where T : class
    {
        IList<T> GetAll();

        T GetById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        List<AgentMetricDto> GetAllMetrics(DateTime time);
    }


}
