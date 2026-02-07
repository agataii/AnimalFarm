using AnimalFarm.Application.DTOs.User;
using AnimalFarm.Application.Interfaces;
using AnimalFarm.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto(
                user.Id,
                user.UserName!,
                user.Email!,
                user.IsActive,
                roles));
        }

        return userDtos;
    }

    public async Task ActivateUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"User with id '{id}' not found.");

        user.IsActive = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task DeactivateUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"User with id '{id}' not found.");

        user.IsActive = false;
        await _userManager.UpdateAsync(user);
    }
}
