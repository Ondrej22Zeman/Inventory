namespace Inventory.Dtos
{
	//DTO resi problemy zmeny vlastnosti objektu v ramci kontraktu s klientem
	public record ItemDto
	{
		//init-setnuti pouze behem inicializace (vytvoreni konstruktorem)
		public Guid Id { get; init; }
		public string Name { get; init; }
		//Decimal je pomalejsi, ale mnohem presnejsi ne float a double (double je 64-bit)
		public decimal Price { get; init; }
		public DateTimeOffset CreatedDate { get; init; }
	}
}
