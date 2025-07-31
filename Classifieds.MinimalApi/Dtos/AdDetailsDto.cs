namespace Classifieds.MinimalApi.Dtos;

public record AdDetailsDto(
    int Id, 
    string Title, 
    string Description, 
    int CategoryId, 
    decimal Price, 
    DateOnly Date);