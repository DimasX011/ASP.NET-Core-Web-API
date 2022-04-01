using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using MetricsManager.DAL.Models;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Response;
using MetricsManager.Client.ApiRequest;

namespace MetricsManager.DAL.Repositories
{
    public interface IAgentIterfaceRepository : IAgentInterface<AgentMetric>
    {

    }

    public class AgentMetricRepository : IAgentIterfaceRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public AgentMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(AgentMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agentmetrics(value, time, adress) VALUES(@value, @time, @adress)",
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
                connection.Execute("DELETE FROM agentmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(AgentMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE agentmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id,
                        adress = item.adress
                    });
            }
        }

        public IList<AgentMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentMetric>("SELECT Id, Time, Value FROM agentmetrics").ToList();
            }
        }

        public AgentMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<AgentMetric>("SELECT Id, Time, Value FROM agentmetrics WHERE id=@id",
                    new { id = id });
            }
        }

        public List<AgentMetricDto> GetAllMetrics(DateTime time)
        {
            List<AgentMetricDto> dateTimes = new List<AgentMetricDto>();
            DateTime fromTime = new DateTime(1970 / 01 / 01);
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id, value, time FROM agentmetrics WHERE time>@fromTime AND time<@toTime;", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes.Add(new AgentMetricDto { Id = reader.GetInt32(0), Time = reader.GetDateTimeOffset(1), Value = reader.GetInt32(2) });
                }
            }
            return dateTimes;
        }

        public GetAllHddMetricsApiRequest GetLastTime()
        {
            GetAllHddMetricsApiRequest dateTimes = new GetAllHddMetricsApiRequest();
            TimeSpan fromTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT adress, time FROM agentmetrics WHERE time = (SELECT MAX(time) FROM agentmetrics WHERE time>@fromTime AND time<@toTime);", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes = new GetAllHddMetricsApiRequest { FromTime = fromTime, ToTime = reader.GetTimeSpan(0), ClientBaseAddress = reader.ToString() };
                }
            }

            return dateTimes;
            //поменять на agentmetric
        }
        }
}
