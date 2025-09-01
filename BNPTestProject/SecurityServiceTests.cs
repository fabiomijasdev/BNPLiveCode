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
        public async Task RetriveIsinsAndSave_WithValidIsins_ReturnsSecuritiesWithPrices()
        {
            // Arrange
            var isins = new List<string> { "US0378331005", "US5949181045" };
            var expectedPrices = new Dictionary<string, decimal>
            {
                { "US0378331005", 150.75m },
                { "US5949181045", 2750.50m }
            };

            _securitiesDataProviderMock
                .Setup(x => x.GetSecurityByIsinAsync("US0378331005"))
                .ReturnsAsync(150.75m);

            _securitiesDataProviderMock
                .Setup(x => x.GetSecurityByIsinAsync("US5949181045"))
                .ReturnsAsync(2750.50m);

            _securityRepositoryMock
                .Setup(x => x.AddOrUpdateBulkAsync(It.IsAny<List<Security>>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _securityService.RetriveIsinsAndSave(isins);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var appleSecurity = result.Find(s => s.ISIN == "US0378331005");
            var microsoftSecurity = result.Find(s => s.ISIN == "US5949181045");

            Assert.NotNull(appleSecurity);
            Assert.NotNull(microsoftSecurity);
            Assert.Equal(150.75m, appleSecurity.Price);
            Assert.Equal(2750.50m, microsoftSecurity.Price);
            Assert.NotEqual(Guid.Empty, appleSecurity.Id);
            Assert.NotEqual(Guid.Empty, microsoftSecurity.Id);

            _securityRepositoryMock.Verify(x => x.AddOrUpdateBulkAsync(It.Is<List<Security>>(s =>
                s.Count == 2 &&
                s.Exists(sec => sec.ISIN == "US0378331005" && sec.Price == 150.75m) &&
                s.Exists(sec => sec.ISIN == "US5949181045" && sec.Price == 2750.50m)
            )), Times.Once);
        }

        [Fact]
        public async Task RetriveIsinsAndSave_WithEmptyIsinsList_ReturnsEmptyList()
        {
            // Arrange
            var emptyIsins = new List<string>();

            // Act
            var result = await _securityService.RetriveIsinsAndSave(emptyIsins);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _securitiesDataProviderMock.Verify(x => x.GetSecurityByIsinAsync(It.IsAny<string>()), Times.Never);
            _securityRepositoryMock.Verify(x => x.AddOrUpdateBulkAsync(It.IsAny<List<Security>>()), Times.Never);
        }
    }
}
