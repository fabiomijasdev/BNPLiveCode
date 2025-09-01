using BNPLiveCode.Api.DataProvider;
using BNPLiveCode.Api.Features.Models;
using BNPLiveCode.Api.Features.Repositories;
using BNPLiveCode.Api.Features.Services;
using Moq;
using Xunit;
using Assert = Xunit.Assert;


namespace BNPTestProject
{
    public  class SecurityServiceTests
    {
        private readonly Mock<ISecurityRepository> _securityRepositoryMock;
        private readonly Mock<ISecuritiesDataProvider> _securitiesDataProviderMock;
        private readonly SecurityService _securityService;

        public SecurityServiceTests()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _securitiesDataProviderMock = new Mock<ISecuritiesDataProvider>();

            _securityService = new SecurityService(
                _securityRepositoryMock.Object,
                _securitiesDataProviderMock.Object,
                new HttpClient()
            );
        }

        [Fact]
        public async Task RetriveIsinsAndSave_ShouldReturnSecurities_WhenIsinsProvided()
        {
            // Arrange
            var isins = new List<string> { "US1234567890", "BR9876543210" };

            _securitiesDataProviderMock
                .Setup(x => x.GetSecurityByIsin(It.IsAny<string>()))
                .Returns(100.50m);

            _securityRepositoryMock
                .Setup(x => x.AddOrUpdateBulkAsync(It.IsAny<List<Security>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _securityService.RetriveIsinsAndSave(isins);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, s => s.ISIN == "US1234567890" && s.Price == 100.50m);
            Assert.Contains(result, s => s.ISIN == "BR9876543210" && s.Price == 100.50m);

            _securityRepositoryMock.Verify(x => x.AddOrUpdateBulkAsync(It.IsAny<List<Security>>()), Times.Once);
        }

        [Fact]
        public async Task RetriveIsinsAndSave_ShouldHandleEmptyList()
        {
            // Arrange
            var isins = new List<string>();

            // Act
            var result = await _securityService.RetriveIsinsAndSave(isins);

            // Assert
            Assert.Empty(result);
            _securityRepositoryMock.Verify(x => x.AddOrUpdateBulkAsync(It.IsAny<List<Security>>()), Times.Once);
        }
    }
}
