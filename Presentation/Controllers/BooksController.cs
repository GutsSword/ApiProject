using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
           var books = _manager.BookService.GetAllBooks(false);
           return Ok(books); 
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var books = _manager.BookService.GetOneBookById(id, false);
            return Ok(books);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {        
          if (book is null)
              return NotFound();  //404

           _manager.BookService.CreateOneBook(book);

           return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoUpdate bookDto)
        {
           if (bookDto is null)
               return BadRequest();  //404

           _manager.BookService.UpdateOneBook(id, bookDto, true);
           return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
           _manager.BookService.DeleteOneBook(id, false);
           return NoContent();           
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //check book?
            var entity = _manager.BookService.GetOneBookById(id, true);

            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, new BookDtoUpdate(),true);

                return NoContent();   //204      
        }
    }
}
