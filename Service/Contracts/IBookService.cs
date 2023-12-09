using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParametres linkParametres, bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);
        Task UpdateOneBookAsync(int id, BookDtoUpdate bookDto, bool trackChanges);
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(BookDtoUpdate bookDtoUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoUpdate bookDtoUpdate, Book book);
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
    }
}
