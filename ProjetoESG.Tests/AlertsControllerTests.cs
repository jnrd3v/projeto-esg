using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Tests
{
    public class AlertsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AlertsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAlerts_ReturnsOkResult_WithPaginatedData()
        {
            // Arrange
            var url = "/alerts";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaginatedAlertViewModel>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(result.PageNumber >= 1);
            Assert.True(result.PageSize >= 1);
            Assert.True(result.TotalCount >= 0);
        }

        [Fact]
        public async Task GetAlerts_WithPagination_ReturnsOkResult()
        {
            // Arrange
            var url = "/alerts?pageNumber=1&pageSize=5";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PaginatedAlertViewModel>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(result);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);
        }

        [Fact]
        public async Task GetAlerts_WithInvalidPageNumber_ReturnsBadRequest()
        {
            // Arrange
            var url = "/alerts?pageNumber=0";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAlerts_WithOversizedPageSize_ReturnsBadRequest()
        {
            // Arrange
            var url = "/alerts?pageSize=200";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
} 