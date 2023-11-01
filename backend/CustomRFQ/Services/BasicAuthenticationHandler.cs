using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    IConfiguration _conf;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration conf)
        : base(options, logger, encoder, clock)
    {
        _conf = conf;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Authorization header is missing.");
        }

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

        if (authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
        {
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            // Implement your custom authentication logic here.
            if (IsValidUser(username, password))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }

        return AuthenticateResult.Fail("Invalid credentials.");
    }

    private bool IsValidUser(string username, string password)
    {
        if (username == _conf.GetValue<string>("BasicAuth:Username") && password == _conf.GetValue<string>("BasicAuth:Password"))
            return true;

        else return false;
    }
}