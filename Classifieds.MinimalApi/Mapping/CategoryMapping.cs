using Classifieds.MinimalApi.Dtos;
using Classifieds.MinimalApi.Entities;

namespace Classifieds.MinimalApi.Mapping;

public static class CategoryMapping
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto(category.Id, category.Name);
    }
}