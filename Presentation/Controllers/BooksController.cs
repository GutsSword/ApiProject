using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services;
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
        public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDto)
        {        
          if (bookDto is null)
              return NotFound();  //404

          if(ModelState.IsValid is false)
            {
                return UnprocessableEntity(ModelState);
            }

          var book = _manager.BookService.CreateOneBook(bookDto);

           return StatusCode(201, book);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoUpdate bookDto)
        {
           if (bookDto is null)
               return BadRequest();  //404

            if (ModelState.IsValid is false)
                return UnprocessableEntity(ModelState);  //422

           _manager.BookService.UpdateOneBook(id, bookDto, false);
           return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
           _manager.BookService.DeleteOneBook(id, false);
           return NoContent();           
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest(); //400

            var result = _manager.BookService.GetOneBookForPatch(id, false);
            bookPatch.ApplyTo(result.bookDtoUpdate, ModelState);

            TryValidateModel(result.bookDtoUpdate);

            if (ModelState.IsValid is false)
                return UnprocessableEntity(ModelState);

            _manager.BookService.SaveChangesForPatch(result.bookDtoUpdate, result.book);
             
                return NoContent();   //204      
        }
    }
}
