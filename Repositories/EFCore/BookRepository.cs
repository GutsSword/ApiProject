﻿using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Sort(bookParameters.OrderBy)
                .Search(bookParameters.SearchTerm)
                .ToListAsync();

            return PagedList<Book>
                .ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _context.Books
                .Include(b => b.Category)   // Lazy Loading yapısından dolayı Include metodu ile Category çağırılmalıdır.
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges)
        {
           return await FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }   

        public void UpdateOneBook(Book book) => Update(book);
    }
}
