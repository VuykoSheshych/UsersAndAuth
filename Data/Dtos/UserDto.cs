namespace UsersAndAuth.Data.Dtos;

public record UserDto(string Id, string Name, int EloRating, string? Avatar = null, string AvatarUrl = "default-avatar.png");