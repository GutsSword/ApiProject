namespace Entities.DataTransferObjects
{

    // You have to  [Serializable] if you code a constructor but it's not recommended.

    
    public record BookDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
    }
}
