namespace AnimalFarm.Application.DTOs.User;

public record UserDto(string Id, string UserName, string Email, bool IsActive, IList<string> Roles);
