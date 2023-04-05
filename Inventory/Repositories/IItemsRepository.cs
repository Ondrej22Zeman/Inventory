using Inventory.Entities;

namespace Inventory.Repositories
{
	public interface IItemsRepository
	{
		//Task - je asynchronni operace ktera EVENTUALNE vrati item AZ se ziska z DB
		Task<Item> GetItemAsync(Guid id);
		Task<IEnumerable<Item>> GetItemsAsync();
		Task CreateItemAsync(Item item);
		Task UpdateItemAsync(Item item);
		Task DeleteItemAsync(Guid id);
	}
}