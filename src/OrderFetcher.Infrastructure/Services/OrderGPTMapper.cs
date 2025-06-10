using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderFetcher.Application.Extensions;
using OrderFetcher.Application.Interfaces;
using OrderFetcher.Application.Prompts;
using OrderFetcher.Application.Responses.GPT;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Infrastructure.Services;

public class OrderGPTMapper : IOrderGPTMapper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderGPTMapper> _logger;
    private readonly string _openAiApiKey;
    private readonly string _openAiEndpoint;

    public OrderGPTMapper(HttpClient httpClient, IConfiguration configuration, ILogger<OrderGPTMapper> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _openAiApiKey = configuration["OpenAI:ApiKey"]
                        ?? throw new ArgumentNullException(nameof(configuration), "OpenAI API key not found.");
        _openAiEndpoint = configuration["OpenAI:Endpoint"]
                          ?? throw new ArgumentNullException(nameof(configuration), "OpenAI Endpoint not found.");
    }

    public async Task<Order?> MapEmailBodyToOrderAsync(string emailBody, CancellationToken cancellationToken = default)
    {
        try
        {
            string prompt = string.Format(GPT.OrderExtractor, emailBody);

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 1000,
                temperature = 0
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            using var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAiApiKey);

            var response = await _httpClient.PostAsync(_openAiEndpoint, requestContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            var rawJsonContent = ExtractJsonFromResponse(responseString);
            if (string.IsNullOrWhiteSpace(rawJsonContent))
                return null;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var orderDto = JsonSerializer.Deserialize<OrderResponse>(rawJsonContent, options);

            if (orderDto == null)
                throw new InvalidOperationException(
                    "Failed to deserialize the OpenAI response into an OrderResponse object.");

            return orderDto.ToOrder();
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Operation canceled while mapping email body to order.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while mapping email body to order.");
            throw;
        }
    }

    private static string? ExtractJsonFromResponse(string responseString)
    {
        using var jsonDoc = JsonDocument.Parse(responseString);
        var root = jsonDoc.RootElement;

        var content = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content?.Trim();
    }
}