using System.Text.Json;


public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAIService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<string> GetResponseFromOpenAI(string prompt)
    {
        var request = new
        {
            model = "gpt-4o-mini", // Specify the model here
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        requestMessage.Headers.Add("Authorization", $"Bearer {_apiKey}");
        requestMessage.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var reply = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return reply ?? string.Empty;
        }

        return "Error: Unable to fetch response from OpenAI.";
    }
}