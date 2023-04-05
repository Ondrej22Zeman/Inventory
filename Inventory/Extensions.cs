using Inventory.Dtos;
using Inventory.Entities;

namespace Inventory
{
    public static class Extensions
    {
		//this znamena ze se to da pouzit na instanci promenne Item, napr item.AsDto()
        public static ItemDto AsDto(this Item item)
        {
			return new ItemDto
			{
				Id = item.Id,
				Name = item.Name,
				Price = item.Price,
				CreatedDate = item.CreatedDate
			};
		}
    }
}
