using System.ComponentModel.DataAnnotations;

namespace Classifieds.MinimalApi.Dtos;

public record UserLoginDto(
    [Required] string Login,
    [Required] string Password);