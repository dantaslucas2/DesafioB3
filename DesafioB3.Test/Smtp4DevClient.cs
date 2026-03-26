using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Test
{
    internal class Smtp4DevClient
    {
        private HttpClient _httpClient;

        public Smtp4DevClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ClearAllAsync(CancellationToken cancellationToken = default)
        {
            var messages = await GetMessagesAsync(cancellationToken);
            foreach (var message in messages)
            {
                using var response = await _httpClient.DeleteAsync($"/api/messages/{message.Id}", cancellationToken);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<List<Smtp4DevMessage>> GetMessagesAsync(CancellationToken cancellationToken = default)
        {
            var messages = await _httpClient.GetFromJsonAsync<List<Smtp4DevMessage>>("/api/messages", cancellationToken);
            return messages ?? Array.Empty<Smtp4DevMessage>();
        }

        public async Task<Smtp4DevMessageDetails?> GetMessageDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<Smtp4DevMessageDetails>($"/api/messages/{id}", cancellationToken);
        }
    }
}
public sealed class Smtp4DevMessage
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public sealed class Smtp4DevMessageDetails
{
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    [JsonPropertyName("textBody")]
    public string TextBody { get; set; } = string.Empty;
}