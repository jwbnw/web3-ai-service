namespace Web3Ai.Service.Authorization;

using Web3Ai.Service.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserDataService userDataService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["X-User-Token"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
               context.Items["User"] = userDataService.GetById(userId.Value); // needs to be a guid
        }

        await _next(context);
    }
}