namespace Api.Auth.Application.Dtos.Responses;

public record TokenResponseDto(string AccessToken, int ExpiresInMinutes);
