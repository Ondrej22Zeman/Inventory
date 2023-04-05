using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos
{
	public record CreateItemDto
	{
		//Name nemuze byt null
		[Required]
		public string Name { get; init; }
		[Required]
		//Cena nemuze byt zaporna
		[Range(1, 1000)]
		public decimal Price { get; init; }
	}
}
