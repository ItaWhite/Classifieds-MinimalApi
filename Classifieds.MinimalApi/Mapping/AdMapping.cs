using Classifieds.MinimalApi.Dtos;
using Classifieds.MinimalApi.Entities;

namespace Classifieds.MinimalApi.Mapping;

public static class AdMapping
{
    public static Ad ToEntity(this CreateAdDto dto)
    {
        return new Ad
        {
            Title = dto.Title,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Price = dto.Price,
            Date = dto.Date
        };
    }

    public static Ad ToEntity(this UpdateAdDto dto, int id)
    {
        return new Ad
        {
            Id = id,
            Title = dto.Title,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Price = dto.Price,
            Date = dto.Date
        };
    }

    public static AdSummaryDto ToSummaryDto(this Ad ad)
    {
        return new AdSummaryDto(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.Category.Name,
            ad.Price,
            ad.Date);
    }

    public static AdDetailsDto ToDetailsDto(this Ad ad)
    {
        return new AdDetailsDto(
            ad.Id,
            ad.Title,
            ad.Description,
            ad.CategoryId,
            ad.Price,
            ad.Date);
    }
}