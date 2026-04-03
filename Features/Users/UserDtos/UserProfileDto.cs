using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Users.UserDtos;

public class UserProfileDto
{
   public required Guid Id { get; set; }
   public required string FirstName { get; set; }
   public required string LastName { get; set; }
   public required string UserName { get; set; }
   public required string Email { get; set; }
   public int AuthoredRessourcesCount { get; set; }
   public int LikedRessourcesCount { get; set; }
   public int FavoriteRessourcesCount { get; set; }
}