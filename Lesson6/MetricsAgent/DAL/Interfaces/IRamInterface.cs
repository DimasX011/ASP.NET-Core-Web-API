﻿using MetricsAgent.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Dal.Interfaces
{
    public interface IRamInterface<T> where T : class
    {
        IList<T> GetAll();

        T GetById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);

        List<RamMetricDto> GetAllMetrics(DateTime time);
    }

}
