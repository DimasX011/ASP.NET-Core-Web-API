using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Client.ApiRequest
{
    public class GetAllDotnetMetricsApiRequest
    {
        public TimeSpan FromTime;

        public TimeSpan ToTime;

        public string ClientBaseAddress;
    }
}
