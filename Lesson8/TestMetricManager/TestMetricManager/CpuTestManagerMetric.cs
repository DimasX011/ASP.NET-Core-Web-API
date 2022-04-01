using System;
using System.Collections.Generic;
using System.Text;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestMetricManager.TestMetricManager
{
    public class CpuTestManagerMetric
    {
        [Fact]
        public void GetMetricsFromAgent()
        {
            //Arrange
            int id = 15466512;
            TimeSpan time = new TimeSpan(12);
            TimeSpan nexttime = new TimeSpan(19);
            var logger = new Mock<ILogger<CpuMetricsController>>();
            var controller = new CpuMetricsController(logger.Object);
            //Act
            var result = controller.GetMetricsFromAgent(id,time,nexttime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster()
        {
            //Arrange
            TimeSpan time = new TimeSpan(12);
            TimeSpan nexttime = new TimeSpan(19);
            var logger = new Mock<ILogger<CpuMetricsController>>();
            var controller = new CpuMetricsController(logger.Object);
            //Act
            var result = controller.GetMetricsFromAllCluster( time, nexttime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
