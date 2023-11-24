using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
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

        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBookById(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);
               
            _manager.Book.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            var books = _manager.Book.GetOneBookById(id,trackChanges );

            if (books is null)
                throw new BookNotFoundException(id); //404

            return books;
        }

        public void UpdateOneBook(int id, BookDtoUpdate bookDto, bool trackChanges)    
        {
            //check entity
            var entity = _manager.Book.GetOneBookById(id,trackChanges);           

            if (entity is null)
                throw new BookNotFoundException(id);

            // Mapping
            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            _manager.Save();
        }   
    }
}
