using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]  // https:localhost:5001/items
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;
        private static int requestCounter = 0;

        public ItemsController(IRepository<Item> _itemsRepository)
        {
            itemsRepository = _itemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync()
        {
            requestCounter++;
            Console.WriteLine($"Request {requestCounter}: Starting ...");

            if (requestCounter <= 2)
            {
                Console.WriteLine($"Request {requestCounter}: Delaying ...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if (requestCounter <=4)
            {
                Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error");
                return StatusCode(500);
            }
            
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            Console.WriteLine($"Request {requestCounter}: 200 (OK).");

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            return (await itemsRepository.GetByIdAsync(id)).AsDto();
        }

        [HttpPost]
        //public void createItem(string name, string description, decimal price)
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            var item = new Item()
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price
            };
            await itemsRepository.CreateItemAsync(item);

            //var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            //items.Add(item);
            //items.Add(new ItemDto(Guid.NewGuid(), name, description, price, DateTimeOffset.UtcNow));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByIdAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetByIdAsync(id);
            if (existingItem == null)
                return NotFound();

            await itemsRepository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}