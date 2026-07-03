using someCrud.domain.dtos;

namespace someCrud.helpers;

public static class PaginationHelper
{
    public static int GetOffset(PaginationFilters filters)
    {
        return (filters.Page-1)*filters.Size;
    }
} 