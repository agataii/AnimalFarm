using AnimalFarm.Application.DTOs.User;

namespace AnimalFarm.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task ActivateUserAsync(string id);
    Task DeactivateUserAsync(string id);
}
