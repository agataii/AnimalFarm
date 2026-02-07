using AnimalFarm.Application.DTOs.Auth;

namespace AnimalFarm.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<bool> ActivateAsync(ActivateDto dto);
}
