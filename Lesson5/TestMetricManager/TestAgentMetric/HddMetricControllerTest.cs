using System;
using System.Collections.Generic;
using System.Text;
using MetricsAgent.AllRequestMetric;
using MetricsAgent.Controllers;
using MetricsAgent.HddMetricRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestMetricManager.TestAgentMetric
{
    public class HddMetricControllerTest
    {
       
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            var request = new CreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 };
            var reposytorymock = new Mock<IhddMetricInterface>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new HddMetricsController(reposytorymock.Object, logger.Object);
            controller.Create(request);
            reposytorymock.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.Once);
        }

        [Fact]
        public void GetMetricsCpuMetricAgent()
        {
            TimeSpan time = new TimeSpan(12);
            TimeSpan nexttime = new TimeSpan(19);
            var reposytorymock = new Mock<IhddMetricInterface>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new HddMetricsController(reposytorymock.Object, logger.Object);
            var result = controller.GetMetricsHddMetricsAgent(time, nexttime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
