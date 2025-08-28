using System.ComponentModel.DataAnnotations;

namespace Classifieds.MinimalApi.Dtos;

public record UserRegistrationDto(
    [Required] string Name, 
    [Required] string Login,
    [Required] string Password);