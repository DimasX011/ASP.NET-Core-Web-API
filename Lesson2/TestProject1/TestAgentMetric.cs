using Xunit;
using MetricsAgent.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;

namespace TestProject1
{
    public class TestAgentMetric
    {
        private CpuMetricsController controller;
        private DotNetMetricsController dotnetcontroller;
        private HddMetricsController HddMetricsController;
        private NetworkMetricsController NetworkMetricsController;
        private RamMetricsController RamMetricsController;

        public TestAgentMetric()
        {
            controller = new CpuMetricsController();
            dotnetcontroller = new DotNetMetricsController();
            HddMetricsController = new HddMetricsController();
            NetworkMetricsController = new NetworkMetricsController();
            RamMetricsController = new RamMetricsController();
        }

        [Fact]
        public void GetMetricsFromCpuMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var result = controller.GetMetricsFromAgent(fromTime, toTime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void GetMetricsFromDotnetMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var dotnetresult = dotnetcontroller.GetMetricsFromAgent(fromTime, toTime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(dotnetresult);
        }
        [Fact]
        public void GetMetricsFromHddMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var hddcontroller = HddMetricsController.GetMetricsFromAgent(fromTime, toTime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(hddcontroller);
        }
        [Fact]
        public void GetMetricsFromNetworkMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var networkres = NetworkMetricsController.GetMetricsFromAgent(fromTime, toTime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(networkres);
        }
        [Fact]
        public void GetMetricsFromRamMetrics_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var RamMetricsres = NetworkMetricsController.GetMetricsFromAgent(fromTime, toTime);
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(RamMetricsres);

        }
    }
}