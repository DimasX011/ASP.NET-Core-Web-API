
using MetricsManager.Client.ApiRequest;
using MetricsManager.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Interfaces
{
    public interface INetworkInterface<T> where T : class
    {
        IList<T> GetAll();

        T GetById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        List<NetworkMetricDto> GetAllMetrics(DateTime time);

        GetAllNetworkApiRequest GetLastTime();
    }

}
