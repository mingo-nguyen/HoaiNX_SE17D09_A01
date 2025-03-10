using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Security.Claims;

public class AccountService : IAccountService
{
    private readonly FunewsManagementContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(FunewsManagementContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SystemAccount> GetCurrentUserAsync()
    {
        var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        return await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var account = await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        if (account != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.AccountEmail),
                new Claim(ClaimTypes.Role, account.AccountRole.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    public async Task UpdateAccountAsync(SystemAccount account)
    {
        _context.SystemAccounts.Update(account);
        await _context.SaveChangesAsync();
    }
}
