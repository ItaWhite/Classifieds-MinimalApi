namespace Classifieds.MinimalApi.Dtos;

public record AdSummaryDto(
    int Id, 
    string Title, 
    string Description, 
    string Category, 
    decimal Price, 
    DateOnly Date,
    string User);