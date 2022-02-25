using MetricsAgent.Controllers;
using MetricsAgent.CpuMetricRepo;
using Moq;
using Xunit;
using System;
using Microsoft.Extensions.Logging;
using MetricsAgent.AllRequestMetric;
using Microsoft.AspNetCore.Mvc;

namespace TestMetricManager.TestAgentMetric
{
    public class CpuMetricsControllerTest
    {
      
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            var request = new CreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 };
            var reposytorymock = new Mock<ICpuInterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new CpuMetricsController(reposytorymock.Object, logger.Object);
            controller.Create(request);
            reposytorymock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.Once);
        }

        [Fact]
        public void GetMetricsCpuMetricAgent()
        {
            TimeSpan time = new TimeSpan(12);
            TimeSpan nexttime = new TimeSpan(19);
            var reposytorymock = new Mock<ICpuInterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new CpuMetricsController(reposytorymock.Object, logger.Object);
            var result = controller.GetMetricsCpuMetricAgent(time,nexttime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

    }



}

