using Database.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Action = Database.Entities.Action;

namespace ActionReactionService.AboutParser;

public interface IAboutParserService
{
    Task ParseAndStoreAboutJsonAsync();
    IReadOnlyList<Service> GetServices();
}

public class AboutParserService : IAboutParserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AboutParserService> _logger;
    private readonly List<Service> _services;

    public AboutParserService(IHttpClientFactory httpClientFactory, ILogger<AboutParserService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ServiceAbout");
        _logger = logger;
        _services = new List<Service>();
    }

    public async Task ParseAndStoreAboutJsonAsync()
    {
        _logger.LogInformation("Fetching about.json...");

        try
        {
            var response = await _httpClient.GetAsync("/about.json");
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var parsedContent = JObject.Parse(jsonContent);

            var servicesJson = parsedContent["server"]?["services"];
            if (servicesJson == null)
            {
                _logger.LogError("No services found in the JSON response.");
                return;
            }

            var services = servicesJson.ToObject<List<ServiceJson>>() ?? new List<ServiceJson>();

            foreach (var serviceJson in services)
            {
                _logger.LogInformation($"Adding service to the in-memory list: {serviceJson.Name}");

                if (_services.Any(s => s.Name == serviceJson.Name))
                {
                    _logger.LogInformation($"Service '{serviceJson.Name}' already exists in memory.");
                    continue;
                }

                var serviceEntity = new Service
                {
                    Name = serviceJson.Name,
                    Actions = serviceJson.Actions.Select(a => new Action
                    {
                        Name = a.Name
                    }).ToList(),
                    Reactions = serviceJson.Reactions.Select(r => new Reaction
                    {
                        Name = r.Name
                    }).ToList()
                };

                _services.Add(serviceEntity);
            }

            _logger.LogInformation("Services successfully added to the in-memory list.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching or parsing about.json.");
        }
    }

    public IReadOnlyList<Service> GetServices()
    {
        return _services.AsReadOnly();
    }
}
