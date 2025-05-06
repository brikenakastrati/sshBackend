
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using Xunit;

namespace sshBackend1.Tests
{
    public class EventControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public EventControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEvents_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Event");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
            apiResponse.Should().NotBeNull();
            apiResponse!.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateEvent_ThenGetEventById_ReturnsCreatedAndRetrieved()
        {
            var newEvent = new EventDTO { EventName = $"Test Event {Guid.NewGuid()}" };

            var postResponse = await _client.PostAsJsonAsync("/api/Event", newEvent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var postApiResponse = await postResponse.Content.ReadFromJsonAsync<APIResponse>();
            var createdJson = (JsonElement)postApiResponse!.Result!;
            var createdEvent = JsonSerializer.Deserialize<EventDTO>(createdJson.GetRawText());

            createdEvent.Should().NotBeNull();

            var getResponse = await _client.GetAsync($"/api/Event/{createdEvent!.EventId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateEvent_ReturnsOk()
        {
            var newEvent = new EventDTO { EventName = $"ToUpdate_{Guid.NewGuid()}" };
            var createResponse = await _client.PostAsJsonAsync("/api/Event", newEvent);
            var createResult = await createResponse.Content.ReadFromJsonAsync<APIResponse>();
            var created = JsonSerializer.Deserialize<EventDTO>(((JsonElement)createResult!.Result!).GetRawText());

            created!.EventName = "Updated Event Name";
            var updateResponse = await _client.PutAsJsonAsync($"/api/Event/{created.EventId}", created);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteEvent_ReturnsOk()
        {
            var newEvent = new EventDTO { EventName = $"ToDelete_{Guid.NewGuid()}" };
            var createResponse = await _client.PostAsJsonAsync("/api/Event", newEvent);
            var createResult = await createResponse.Content.ReadFromJsonAsync<APIResponse>();
            var created = JsonSerializer.Deserialize<EventDTO>(((JsonElement)createResult!.Result!).GetRawText());

            var deleteResponse = await _client.DeleteAsync($"/api/Event/{created!.EventId}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PatchEvent_UpdatesEventName()
        {
            var newEvent = new EventDTO { EventName = $"PatchTest_{Guid.NewGuid()}" };
            var createResponse = await _client.PostAsJsonAsync("/api/Event", newEvent);
            var createResult = await createResponse.Content.ReadFromJsonAsync<APIResponse>();
            var created = JsonSerializer.Deserialize<EventDTO>(((JsonElement)createResult!.Result!).GetRawText());

            var patchDoc = new[]
            {
                new Dictionary<string, object>
                {
                    ["op"] = "replace",
                    ["path"] = "/EventName",
                    ["value"] = "Patched Event Name"
                }
            };

            var patchContent = new StringContent(JsonSerializer.Serialize(patchDoc), Encoding.UTF8, "application/json-patch+json");

            var patchResponse = await _client.PatchAsync($"/api/Event/{created!.EventId}", patchContent);

            patchResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
