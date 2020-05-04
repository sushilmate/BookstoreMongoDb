using BookstoreMongoDb.Models;
using BookstoreMongoDb.Models.BooksApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookstoreMongoDb.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            var books = await _books.FindAsync(book => true);
            return await books.ToListAsync();
        }

        public async Task<Book> GetAsync(string id)
        {
            var book = await _books.FindAsync(book => book.Id == id);

            return await book.FirstOrDefaultAsync();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task UpdateAsync(string id, Book bookIn)
        {
            await _books.ReplaceOneAsync(book => book.Id == id, bookIn);
        }

        public async Task RemoveAsync(Book bookIn)
        {
           await _books.DeleteOneAsync(book => book.Id == bookIn.Id);
        }

        public async Task RemoveAsync(string id)
        {
            await _books.DeleteOneAsync(book => book.Id == id);
        }
    }
}