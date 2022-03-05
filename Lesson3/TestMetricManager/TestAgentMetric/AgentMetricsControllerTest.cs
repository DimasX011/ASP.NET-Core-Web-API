using MetricsAgent.Controllers;
using MetricsAgent.AgentmetricRepo;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using MetricsAgent.AgentMetricRepo;
using Xunit;
using Microsoft.Extensions.Logging;
using MetricsAgent.AllRequestMetric;
using MetricsAgent;
using Microsoft.AspNetCore.Mvc;

namespace TestMetricManager.TestAgentMetric
{
   public class AgentMetricsControllerTest
    {
    
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            var request = new CreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 };
            var reposytorymock = new Mock<IAgentIterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(reposytorymock.Object, logger.Object);
            controller.Create(request);
            reposytorymock.Verify(repository => repository.Create(It.IsAny<AgentMetric>()), Times.Once);


        }


        [Fact]
        public void RegisterAgent()
        {
            var agent = new AgentInfo(25,new Uri("https://vk.com/feed"));
            var reposytorymock = new Mock<IAgentIterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(reposytorymock.Object, logger.Object);
            var result = controller.RegisterAgent(agent);
            Assert.IsAssignableFrom<IActionResult>(result);
        }


        [Fact]
        public void EnableAgentById()
        {
            int id = 11014121;
            var reposytorymock = new Mock<IAgentIterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(reposytorymock.Object, logger.Object);
            var result = controller.EnableAgentById(id);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById()
        {
            int id = 11014121;
            var reposytorymock = new Mock<IAgentIterfaceRepository>();
            var logger = new Mock<ILogger<AgentMetricsController>>();
            var controller = new AgentMetricsController(reposytorymock.Object, logger.Object);
            var result = controller.DisableAgentById(id);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
