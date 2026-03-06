namespace Ressource_API.Features.Authentifications.AuthentificationDtos;

public class LoginResponseDTO
{
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
}