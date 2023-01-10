using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
    // General info for each Item in the Catalog that can be returned to the client app
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    // Items needed from client-side to create a new Item in the catalog
    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    // Items needed from client-side to update an existing Item in the catalog
    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);
}