using System.IdentityModel.Tokens.Jwt;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace AuthService;

public class UserResetPasswordEventHandler : IIntegrationEventHandler<UserResetPasswordEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    
    public UserResetPasswordEventHandler(IDatabaseHandler dbHandler) => _dbHandler = dbHandler;
    
    public async Task<(string, ResultType)> HandleAsync(UserResetPasswordEvent @event)
    {
        string? userId = GetJwtSubClaim(@event.JwtToken);

        if (userId == null)
        {
            return ("Not logged in.", ResultType.Fail);
        }

        Guid parsedUserId = Guid.Parse(userId);

        var user = (await _dbHandler.GetAsync<User>(s => s.Id == parsedUserId)).FirstOrDefault();

        if (user == null || user.Id != parsedUserId)
        {
            return ("Not logged in.", ResultType.Fail);
        }
        
        bool validPassword = @event.Password.VerifyPassword(user.Salt, user.Password);

        if (!validPassword)
        {
            return ("Verify your credentials.", ResultType.Fail);
        }
        
        string password = @event.NewPassword.HashPassword(out string salt);

        user.Password = password;
        user.Salt = salt;
        await _dbHandler.UpdateAsync(user);
        return ("OK", ResultType.Success);
    }
    
    public string? GetJwtSubClaim(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT token.");
        }

        var jwtToken = handler.ReadJwtToken(token);

        var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        return subClaim?.Value;
    }
}