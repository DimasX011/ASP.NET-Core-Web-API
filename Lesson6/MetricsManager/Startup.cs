using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Jobs;
using MetricsManager.Client;
using MetricsManager.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Jobs.BasicJobs;
using MetricsManager.Jobs.AgentJobs;

namespace MetricsManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;";

        // This method gets called by the runtime. Use this method to add services to the container.
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
            cronExpression: "0/15 * * * * ?"));
            

            services.AddSingleton<RamMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(RamMetricJob),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<AgentMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(AgentMetricJob),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<DotnetMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(DotnetMetricJob),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<NetworkMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(NetworkMetricJob),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<HddMetricJob>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(HddMetricJob),
            cronExpression: "0/15 * * * * ?"));

            //call Task IMetricsAgentClient

            services.AddSingleton<CpuMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(CpuMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<AgentMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(AgentMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<DotnetMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(DotnetMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<HddMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(HddMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<NetworkMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(NetworkMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

            services.AddSingleton<RamMetricJobAgent>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(RamMetricJobAgent),
            cronExpression: "0/15 * * * * ?"));

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

            services.AddControllers();
            services.AddSingleton<AgentInfo>();
            services.AddHttpClient();

            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _=> TimeSpan.FromMilliseconds(1000)));
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
