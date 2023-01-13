using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient httpClient;
        
        public CatalogClient(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
            return items;   
        }
    }
}