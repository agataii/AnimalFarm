namespace AnimalFarm.Application.Interfaces;

public interface IUserLookupService
{
    Task<string?> GetUserNameByIdAsync(string userId);
    Task<bool> IsInRoleAsync(string userId, string role);
}
