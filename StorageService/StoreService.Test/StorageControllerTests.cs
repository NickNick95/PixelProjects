
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Storage.Data.Repositories;
using StorageService.Controllers;

namespace StoreService.Test
{
    [TestClass]
    public class StorageControllerTests
    {
        [TestMethod]
        public async Task GetLogs_Sucsees()
        {
            const string log = "01.01.01 | 111 | 111 | 111 |";
            const string fileName = "logs.txt";

            // Arrange
            var mockFileRepo = new Mock<IFileRepository>();
            mockFileRepo.Setup(repo => repo.GetLogsFromTmpFile(fileName))
                .Returns(log);


            var inMemorySettings = new Dictionary<string, string> {
                {"LogFileName", fileName},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();


            var controller = new StorageController(mockFileRepo.Object, configuration);

            // Act
            var actionResult = controller.GetLogs();

            // Assert

            Assert.IsNotNull(actionResult);
            var result = actionResult as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(log, result.Value);
        }
    }
}