using System.IdentityModel.Tokens.Jwt;

namespace Extension;

public static class JwtExtension
{
    public static string GetJwtSubClaim(this string token)
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