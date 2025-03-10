using Domain;

public interface IAccountService
{
    Task<SystemAccount> GetCurrentUserAsync();
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task UpdateAccountAsync(SystemAccount account); // Add this line
}
