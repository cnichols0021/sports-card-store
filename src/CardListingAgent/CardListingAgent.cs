using Microsoft.Extensions.Logging;
using SportsCardStore.Core.Enums;
using SportsCardStore.Shared.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardListingAgent;

/// <summary>
/// AI agent that processes raw card descriptions using the Anthropic Claude API
/// to generate structured CardListing objects with title, description, price suggestion, tags, and category.
/// </summary>
public class CardListingAgent
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CardListingAgent> _logger;
    private readonly string _apiKey;
    private const string ClaudeApiUrl = "https://api.anthropic.com/v1/messages";
    private const string ClaudeModel = "claude-sonnet-4-6";

    public CardListingAgent(HttpClient httpClient, ILogger<CardListingAgent> logger, string apiKey)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        // Configure HttpClient for Anthropic API
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Processes a raw card description and returns a structured CardListing object
    /// </summary>
    /// <param name="rawDescription">Raw text description of the sports card</param>
    /// <param name="cancellationToken">Cancellation token for async operation</param>
    /// <returns>Structured CardListing with AI-generated title, description, price, tags, and category</returns>
    public async Task<CardListing?> ProcessCardDescriptionAsync(string rawDescription, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(rawDescription))
        {
            _logger.LogWarning("Raw description is null or empty");
            return null;
        }

        _logger.LogInformation("Processing card description: {Description}",
            rawDescription.Substring(0, Math.Min(rawDescription.Length, 100)));

        try
        {
            var prompt = BuildPrompt(rawDescription);
            var response = await CallClaudeApiAsync(prompt, cancellationToken);

            if (response == null)
            {
                _logger.LogError("Received null response from Claude API");
                return null;
            }

            var cardListing = ParseClaudeResponse(response);

            if (cardListing != null)
                _logger.LogInformation("Successfully processed card listing: {Title}", cardListing.Title);
            else
                _logger.LogWarning("Failed to parse Claude response into CardListing object");

            return cardListing;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error when calling Claude API");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request to Claude API was cancelled or timed out");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing JSON response from Claude API");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing card description");
            return null;
        }
    }

    private string BuildPrompt(string rawDescription)
    {
        var availableCategories = string.Join(", ", Enum.GetNames<Category>());

        return $@"You are an expert sports card analyst. Analyze the following raw card description and create a structured listing.

Raw Description: ""{rawDescription}""

Please respond with ONLY a JSON object containing these fields:
- Title: A concise, professional title for the card listing (max 200 characters)
- Description: A detailed description including condition, features, and collector appeal (max 2000 characters)
- SuggestedPrice: A realistic price in USD based on current market conditions (decimal number)
- Tags: An array of relevant search tags (strings)
- Category: One of these exact values: {availableCategories}

Example format:
{{
  ""Title"": ""2023 Topps Chrome Ronald Acuna Jr. Base Card #1"",
  ""Description"": ""Beautiful base card featuring Ronald Acuna Jr. in excellent condition. The chrome finish provides fantastic eye appeal with sharp corners and excellent centering. A must-have for any Braves collector or Acuna fan."",
  ""SuggestedPrice"": 12.50,
  ""Tags"": [""Ronald Acuna Jr"", ""Braves"", ""Rookie"", ""Chrome"", ""2023 Topps""],
  ""Category"": ""Baseball""
}}

Respond with ONLY the JSON object, no additional text.";
    }

    private async Task<string?> CallClaudeApiAsync(string prompt, CancellationToken cancellationToken)
    {
        try
        {
            var requestBody = new
            {
                model = ClaudeModel,
                max_tokens = 1000,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogDebug("Sending request to Claude API using model {Model}", ClaudeModel);

            var response = await _httpClient.PostAsync(ClaudeApiUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Claude API returned error status {StatusCode}: {ErrorContent}",
                    response.StatusCode, errorContent);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Received response from Claude API");

            // Parse Claude's response envelope to extract the text content
            var claudeResponse = JsonSerializer.Deserialize<ClaudeApiResponse>(responseContent);
            return claudeResponse?.Content?.FirstOrDefault()?.Text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Claude API");
            return null;
        }
    }

    private CardListing? ParseClaudeResponse(string response)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                _logger.LogWarning("Claude response is null or empty");
                return null;
            }

            // Strip any markdown code fences Claude may have added despite instructions
            var cleanedResponse = response.Trim();
            if (cleanedResponse.StartsWith("```json"))
                cleanedResponse = cleanedResponse[7..];
            if (cleanedResponse.StartsWith("```"))
                cleanedResponse = cleanedResponse[3..];
            if (cleanedResponse.EndsWith("```"))
                cleanedResponse = cleanedResponse[..^3];
            cleanedResponse = cleanedResponse.Trim();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var cardListing = JsonSerializer.Deserialize<CardListing>(cleanedResponse, options);

            if (cardListing != null && ValidateCardListing(cardListing))
                return cardListing;

            _logger.LogWarning("Parsed CardListing failed validation");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing Claude response: {Response}", response);
            return null;
        }
    }

    private bool ValidateCardListing(CardListing cardListing)
    {
        if (string.IsNullOrWhiteSpace(cardListing.Title) || cardListing.Title.Length > 200)
        {
            _logger.LogWarning("Invalid title: null, empty, or exceeds 200 characters");
            return false;
        }

        if (string.IsNullOrWhiteSpace(cardListing.Description) || cardListing.Description.Length > 2000)
        {
            _logger.LogWarning("Invalid description: null, empty, or exceeds 2000 characters");
            return false;
        }

        if (cardListing.SuggestedPrice <= 0 || cardListing.SuggestedPrice > 999999.99m)
        {
            _logger.LogWarning("Invalid suggested price: {Price}", cardListing.SuggestedPrice);
            return false;
        }

        if (!Enum.IsDefined<Category>(cardListing.Category))
        {
            _logger.LogWarning("Invalid category: {Category}", cardListing.Category);
            return false;
        }

        return true;
    }
}

/// <summary>
/// Response envelope returned by the Anthropic Claude API
/// </summary>
internal class ClaudeApiResponse
{
    [JsonPropertyName("content")]
    public List<ClaudeContent>? Content { get; set; }
}

/// <summary>
/// Individual content block within a Claude API response
/// </summary>
internal class ClaudeContent
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
