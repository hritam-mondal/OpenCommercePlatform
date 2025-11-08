namespace OCP.API.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var idValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("sub")?.Value
                      ?? user.FindFirst("uid")?.Value
                      ?? user.FindFirst("userid")?.Value;

        if (string.IsNullOrWhiteSpace(idValue))
        {
            return null;
        }

        if (int.TryParse(idValue, out var id))
        {
            return id;
        }

        if (long.TryParse(idValue, out var id64))
        {
            return checked((int)id64);
        }

        return null;
    }
}
