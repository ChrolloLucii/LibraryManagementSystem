using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public class LibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService()
        {
            _context = new LibraryContext();
        }

        // Авторы
        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task AddAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
           var existingAuthor = await _context.Authors.FindAsync(author.Id);
    if (existingAuthor != null)
    {

        _context.Entry(existingAuthor).CurrentValues.SetValues(author);
    }
    else
    {
        _context.Authors.Attach(author);
        _context.Entry(author).State = EntityState.Modified;
    }
    
    await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        // Жанры
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

       public async Task UpdateGenreAsync(Genre genre)
{
 
    var existingGenre = await _context.Genres.FindAsync(genre.Id);
    if (existingGenre != null)
    {
 
        _context.Entry(existingGenre).CurrentValues.SetValues(genre);
    }
    else
    {
        _context.Genres.Attach(genre);
        _context.Entry(genre).State = EntityState.Modified;
    }
    
    await _context.SaveChangesAsync();
}

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }

        //книги
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
           var existingBook = await _context.Books.FindAsync(book.Id);
           if (existingBook != null){
            _context.Entry(existingBook).CurrentValues.SetValues(book);
           }
           else {
            _context.Books.Attach(book);
            _context.Entry(book).State = EntityState.Modified;
           }
           await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}