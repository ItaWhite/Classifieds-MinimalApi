using System.ComponentModel.DataAnnotations;

namespace Classifieds.MinimalApi.Dtos;

public record CreateAdDto(
    [Required][StringLength(50)] string Title, 
    [Required] string Description, 
    [Required] int CategoryId, 
    [Required][Range(10, 1000_000_000)] decimal Price, 
    [Required] DateOnly Date);