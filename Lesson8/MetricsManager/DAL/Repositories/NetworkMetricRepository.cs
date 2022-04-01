using Dapper;
using MetricsAgent;
using MetricsManager.Client.ApiRequest;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.Response;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public interface INetworkMetricRepoitory : INetworkInterface<NetworkMetric>
    {

    }
    public class NetworkMetricRepository : INetworkMetricRepoitory
    {

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public NetworkMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(NetworkMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO networkmetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        adress = item.adress

                    });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM networkmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(NetworkMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE networkmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                    });
            }
        }

        public IList<NetworkMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetric>("SELECT Id, Time, Value FROM networkmetrics").ToList();
            }
        }

        public NetworkMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<NetworkMetric>("SELECT Id, Time, Value FROM networkmetrics WHERE id=@id",
                    new { id = id });
            }
        }

        public List<NetworkMetricDto> GetAllMetrics(DateTime time)
        {
            List<NetworkMetricDto> dateTimes = new List<NetworkMetricDto>();
            DateTime fromTime = new DateTime(1970 / 01 / 01);
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id, value, time FROM networkmetrics WHERE time>@fromTime AND time<@toTime;", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes.Add(new NetworkMetricDto { Id = reader.GetInt32(0), Time = reader.GetDateTimeOffset(1), Value = reader.GetInt32(2) });
                }
            }
            return dateTimes;
        }

        public GetAllNetworkApiRequest GetLastTime()
        {
            GetAllNetworkApiRequest dateTimes = new GetAllNetworkApiRequest();
            TimeSpan fromTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT adress, time FROM networkmetrics WHERE time = (SELECT MAX(time) FROM networkmetrics WHERE time>@fromTime AND time<@toTime);", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes = new GetAllNetworkApiRequest { FromTime = fromTime, ToTime = reader.GetTimeSpan(0), ClientBaseAddress = reader.ToString() };
                }
            }
            return dateTimes;
        }
    }
}
