using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]    // ...(typeof(LogFilterAttribute), Order = 2). Order word point to priority between Attributes.
    [Route("api/books")]
    //[ResponseCache(CacheProfileName="5 Min")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        //[ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)   // ***[FromQuery]*** example= /books?pageNumber=2&pageSize=10
        {
            var linkParametres = new LinkParametres()
            {
                BookParameters = bookParameters,
                HttpContext = HttpContext
            };
           var result = await _manager.BookService.GetAllBooksAsync(linkParametres , false);

           Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));  // Add to Header
            return result.linkResponse.HasLinks ?
                 Ok(result.linkResponse.LinkedEntities) :
                 Ok(result.linkResponse.ShapedEntities) ;
        }

        [HttpGet("{id:int}")] 
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var books = await _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(books);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager.BookService.GetAllBooksWithDetails(false));
        }

        [HttpPost(Name = "CreateOneBookAsync")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]     
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {               
          var book = await _manager.BookService.CreateOneBookAsync(bookDto);

           return StatusCode(201, book);
        }
          
        
        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoUpdate bookDto)
        {
           await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
           return NoContent(); //204
        }
       

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
           await _manager.BookService.DeleteOneBookAsync(id, false);
           return NoContent();           
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest(); //400

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);
            bookPatch.ApplyTo(result.bookDtoUpdate, ModelState);

            TryValidateModel(result.bookDtoUpdate);

            if (ModelState.IsValid is false)
                return UnprocessableEntity(ModelState);

           await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoUpdate, result.book);
             
                return NoContent();   //204      
        }


        [HttpOptions]
        public IActionResult GetBookOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
