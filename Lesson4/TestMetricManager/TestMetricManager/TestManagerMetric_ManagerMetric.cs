using System;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestMetricManager.TestMetricManager
{
    public class TestManagerMetric_ManagerMetric
    {
        


        [Fact]
        public void RegisterAgentt_ReturnsOk()
        {
            //Arrange
            var info = new MetricsManager.AgentInfo();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(logger.Object);
            //Act
            var result = controller.RegisterAgent(info);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult> (result);
        }


        [Fact]
        public void EnableAgentByIdd_ReturnsOk()
        {
            //Arrange
            int info = 2;
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(logger.Object);
            //Act
            var result = controller.EnableAgentById(info);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentByIdd_ReturnsOk()
        {
            //Arrange
            int info = 5;
            //Act
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(logger.Object);
            //Act
            var result = controller.EnableAgentById(info);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}
