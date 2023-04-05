namespace Inventory.Entities
{
	//Dobre pro praci s immutable objekty (jedna instance)
	public record Item
	{
		//init-setnuti pouze behem inicializace (vytvoreni konstruktorem)
		public Guid Id { get; init; }
		public string Name { get; init; }
		//Decimal je pomalejsi, ale mnohem presnejsi ne float a double (double je 64-bit)
		public decimal Price { get; init; }
		public DateTimeOffset CreatedDate { get; init; }
	}
}
