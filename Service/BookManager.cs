using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Expections;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private  readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IBookLinks _bookLinks;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
        }    

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParametres linkParametres, bool trackChanges)
        {
            if (linkParametres.BookParameters.ValidPriceRange is false)
                throw new PriceOutofRangeBadRequestException();

            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(linkParametres.BookParameters,trackChanges);
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            var links = _bookLinks.TryGenerateLinks(booksDto,
                linkParametres.BookParameters.Fields,
                linkParametres.HttpContext);

            return (links,booksWithMetaData.MetaData);  //Tupple Struct
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var books = await GetOneBookIdAndCheckExists(id,trackChanges);
            return _mapper.Map<BookDto>(books);
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var category = _manager.Category.GetCategoryByIdAsync(bookDto.CategoryId, false);

            if (category is null)
                throw new CategoryNotFoundException(bookDto.CategoryId);

            var entity = _mapper.Map<Book>(bookDto);
            entity.CategoryId=bookDto.CategoryId;
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {

            var entity = await GetOneBookIdAndCheckExists(id, trackChanges);

            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<(BookDtoUpdate bookDtoUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookIdAndCheckExists(id, trackChanges);
            var bookDtoForUpdate = _mapper.Map<BookDtoUpdate>(book);

            return (bookDtoForUpdate, book);

        }

        public async Task SaveChangesForPatchAsync(BookDtoUpdate bookDtoUpdate, Book book)
        {
            _mapper.Map(bookDtoUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id, BookDtoUpdate bookDto, bool trackChanges)    
        {
            var entity = await GetOneBookIdAndCheckExists(id, trackChanges);
            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _manager.Book.GetAllBooksAsync(trackChanges);
            return books;
        }

        // Check Method
        private async Task<Book> GetOneBookIdAndCheckExists(int id , bool trackChanges)
        {
            //check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            return entity;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetails(bool trackChanges)
        {
            return await _manager.Book.GetAllBooksWithDetailsAsync(trackChanges);
        }
    }
}