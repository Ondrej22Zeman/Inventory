using Inventory.Dtos;
using Inventory.Entities;
using Inventory.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
	[ApiController]
	//[Route("[controller]")] //takhle bude cesta /items
	[Route("items")]
	public class ItemsController : ControllerBase
	{
		private readonly IItemsRepository _repository;

		public ItemsController(IItemsRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<IEnumerable<ItemDto>> GetItemsAsync()
		{
			var items = (await _repository.GetItemsAsync())
						.Select(item => item.AsDto());
			return items;
		}

		[HttpGet("{id}")] // /items/{id}
		public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
		{
			var item = await _repository.GetItemAsync(id);

			if (item is null)
			{
				return NotFound();
			}

			return item.AsDto();
		}
		[HttpPost]
		public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
		{
			Item item = new Item()
			{
				Id = Guid.NewGuid(),
				Name = itemDto.Name,
				Price = itemDto.Price,
				CreatedDate = DateTimeOffset.UtcNow
			};

			await _repository.CreateItemAsync(item);

			return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
		}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
		{
			var existingItem = await _repository.GetItemAsync(id);
			if (existingItem is null) return NotFound();

			//with znamena vytvareni kopie existing itemu kde se meni jen name a price
			Item updatedItem = existingItem with
			{
				Name = itemDto.Name,
				Price = itemDto.Price
			};

			await _repository.UpdateItemAsync(updatedItem);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteItemAsync(Guid id) 
		{
			var existingItem = await _repository.GetItemAsync(id);
			if (existingItem is null) return NotFound();

			await _repository.DeleteItemAsync(id);

			return NoContent();
		}
	}
}
