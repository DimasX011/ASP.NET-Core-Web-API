
using AutoMapper;
using Core;
using FluentMigrator.Runner;
using MetricsAgent.Dal.Interfaces;
using MetricsAgent.Dal.Repositories;
using MetricsAgent.Dal.Models;
using MetricsAgent.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data.SQLite;
using MetricsAgent.AgentMetricRepo;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ICpuInterfaceRepository, CpuMetricRepository>();
            services.AddSingleton<IRamMetricRepository, RamMetricRepository>();
            services.AddSingleton<IDotnetInterfaceRepository, DotnetMetricRepository>();
            services.AddSingleton<IhddMetricInterface, HddMetricRepository>();
            services.AddSingleton<IAgentIterfaceRepository, AgentMetricRepository>();
            services.AddSingleton<INetworkMetricRepoitory, NetworkMetricRepository>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton<CpuMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(CpuMetricJob),
            cronExpression: "0/5 * * * * ?")); // ��������� ������ 5 ������

            services.AddSingleton<RamMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(RamMetricJob),
            cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<AgentMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(AgentMetricJob),
            cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<DotnetMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(DotnetMetricJob),
            cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<NetworkMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(NetworkMetricJob),
            cronExpression: "0/5 * * * * ?"));

            services.AddSingleton<HddMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(HddMetricJob),
            cronExpression: "0/5 * * * * ?"));



            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // ��������� ��������� SQLite 
                    .AddSQLite()
                    // ������������� ������ �����������
                    .WithGlobalConnectionString(ConnectionString)
                    // ������������, ��� ������ ������ � ����������
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            migrationRunner.MigrateUp();
        }

    }
}
