using System.IdentityModel.Tokens.Jwt;
using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;

namespace ActionReactionService;

public class GetServicesEventHandler : IIntegrationEventHandler<GetServiceEvent, (List<Service>, ResultType)>
{
    private readonly ServiceService _serviceService;

    public GetServicesEventHandler(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(GetServiceEvent @event)
    {
        string id = GetJwtSubClaim(@event.JwtToken);

        if (Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var services = await _serviceService.FindServicesAsync(s => s.UserId == userId);
        
        return (services.ToList(), ResultType.Success);
    }
    
    public string GetJwtSubClaim(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT token.");
        }

        var jwtToken = handler.ReadJwtToken(token);

        var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (subClaim == null)
        {
            throw new InvalidOperationException("The 'sub' claim is missing from the token.");
        }

        return subClaim;
    }
}