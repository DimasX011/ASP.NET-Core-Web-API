﻿using Dapper;
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
    public interface IhddMetricInterface : IHddInterface<HddMetric>
    {

    }
    public class HddMetricRepository : IhddMetricInterface
    {

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public HddMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(HddMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO hddmetrics(value, time, adress) VALUES(@value, @time, @adress)",
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
                connection.Execute("DELETE FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(HddMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id", 
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                        
                    });
            }
        }

        public IList<HddMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetric>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }

        public HddMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<HddMetric>("SELECT Id, Time, Value FROM hddmetrics WHERE id=@id",
                    new { id = id });
            }
        }

        public List<HddMetricDto> GetAllMetrics(DateTime time)
        {
            List<HddMetricDto> dateTimes = new List<HddMetricDto>();
            DateTime fromTime = new DateTime(1970 / 01 / 01);
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id, value, time FROM hddmetrics WHERE time>@fromTime AND time<@toTime;", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes.Add(new HddMetricDto { Id = reader.GetInt32(0), Time = reader.GetDateTimeOffset(1), Value = reader.GetInt32(2) });
                }
            }
            return dateTimes;
        }

        public GetAllHddMetricsApiRequest GetLastTime()
        {
            GetAllHddMetricsApiRequest dateTimes =new  GetAllHddMetricsApiRequest ();
            TimeSpan fromTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT adress, time FROM hddmetrics WHERE time = (SELECT MAX(time) FROM hddmetrics WHERE time>@fromTime AND time<@toTime);", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dateTimes = new GetAllHddMetricsApiRequest { FromTime = fromTime, ToTime = reader.GetTimeSpan(0), ClientBaseAddress = reader.ToString() };
                }
            }

            return dateTimes;
        }


    }
}
