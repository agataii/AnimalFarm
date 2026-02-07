namespace AnimalFarm.Application.DTOs.Auth;

public record AuthResponseDto(string Token, string UserName, IList<string> Roles, DateTime Expiration);
