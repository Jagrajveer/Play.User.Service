using System.ComponentModel.DataAnnotations;

namespace Play.User.Service.Dtos;

public record UserDto(Guid Id, string FirstName, string LastName, string UserName, string Email, DateTimeOffset CreatedDate);

public record UserCreateDto([Required] string FirstName, [Required] string LastName, [Required] string UserName, [Required] string Email);

public record UserUpdateDto(string FirstName, string LastName, string UserName, string Email);
