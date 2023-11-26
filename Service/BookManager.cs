using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
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

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
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

        public async Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(bookParameters,trackChanges);
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
            return (booksDto, booksWithMetaData.MetaData);  //Tupple Struct
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var books = await GetOneBookIdAndCheckExists(id,trackChanges);
            return _mapper.Map<BookDto>(books);
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






        // Check Method
        private async Task<Book> GetOneBookIdAndCheckExists(int id , bool trackChanges)
        {
            //check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            return entity;
        }

    }
}
