using Dapper;
using MetricsAgent.Dal.Interfaces;
using MetricsAgent.Dal.Models;
using MetricsAgent.Response;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Dal.Repositories
{
    public interface IDotnetInterfaceRepository : IDotnetInterface<DotnetMetric>
    {

    }
    public class DotnetMetricRepository : IDotnetInterfaceRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public DotnetMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(DotnetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds
                    });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM dotnetmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(DotnetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE dotnetmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                    });
            }
        }

        public IList<DotnetMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotnetMetric>("SELECT Id, Time, Value FROM dotnetmetrics").ToList();
            }
        }

        public DotnetMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<DotnetMetric>("SELECT Id, Time, Value FROM dotnetmetrics WHERE id=@id",
                    new { id = id });
            }
        }

        public List<DotnetMetricDto> GetAllMetrics(DateTime time)
        {
            List<DotnetMetricDto> dateTimes = new List<DotnetMetricDto>();
            DateTime fromTime = new DateTime(1970 / 01 / 01);
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id, value, time FROM dotnetmetrics WHERE time>@fromTime AND time<@toTime;", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes.Add(new DotnetMetricDto { Id = reader.GetInt32(0), Time = reader.GetDateTimeOffset(1), Value = reader.GetInt32(2) });
                }
            }
            return dateTimes;
        }
    }
}
