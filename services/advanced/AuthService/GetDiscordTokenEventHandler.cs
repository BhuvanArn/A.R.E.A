using System.Text;
using System.Text.Json;
using EventBus;
using EventBus.Event;

namespace AuthService;

public class GetDiscordTokenEventHandler : IIntegrationEventHandler<GetDiscordTokenEvent, (string, ResultType)>
{
    public async Task<(string, ResultType)> HandleAsync(GetDiscordTokenEvent @event)
    {
        var data = new
        {
            login = @event.Email,
            password = @event.Password,
            undelete = false,
            login_source = (string?)null,
            gift_code_sku_id = (string?)null
        };
        
        var jsonData = JsonSerializer.Serialize(data);
        
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("accept", "*/*");
        client.DefaultRequestHeaders.Add("accept-language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        client.DefaultRequestHeaders.Add("dnt", "1");
        client.DefaultRequestHeaders.Add("origin", "https://discord.com");
        client.DefaultRequestHeaders.Add("pragma", "no-cache");
        client.DefaultRequestHeaders.Add("priority", "u=1, i");
        client.DefaultRequestHeaders.Add("referer", "https://discord.com/login");
        client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.6613.178 Safari/537.36");
        client.DefaultRequestHeaders.Add("x-debug-options", "bugReporterEnabled");
        client.DefaultRequestHeaders.Add("x-discord-locale", "fr");
        client.DefaultRequestHeaders.Add("x-discord-timezone", "Europe/Paris");
        
        try
        {
            var response = await client.PostAsync(
                "https://discord.com/api/v9/auth/login",
                new StringContent(jsonData, Encoding.UTF8, "application/json")
            );

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(responseContent);
                if (doc.RootElement.TryGetProperty("token", out var tokenElement))
                {
                    string token = tokenElement.GetString()!;
                    return (token, ResultType.Success);
                }

                return ("Token not found in the response.", ResultType.Fail);
            }
            else
            {
                return ($"Error: {response.StatusCode} - {responseContent}", ResultType.Fail);
            }
        }
        catch (Exception ex)
        {
            return (ex.Message, ResultType.Fail);
        }
    }
}