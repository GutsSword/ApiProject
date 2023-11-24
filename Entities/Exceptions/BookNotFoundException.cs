namespace Entities.Expections
{
    // Sealed class unusable for inheritance.
    public sealed class BookNotFoundException : NotFoundException 
    {
        public BookNotFoundException(int id) : base($"The book with id : {id} could not found.")
        {
            
        }
    }
}
