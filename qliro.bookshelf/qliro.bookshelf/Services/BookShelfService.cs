using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qliro.bookshelf.Data;
using qliro.bookshelf.Models;

namespace qliro.bookshelf.Services
{
    public class BookShelfService
    {
        private readonly ApplicationContext _context;

        public BookShelfService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
            => _context.Books;

        public async Task<User> FindUserAsync(string userId)
            => await _context.Users.FindAsync(userId);

        public async Task<Book> FindBookAsync(int bookId)
            => await _context.Books.FindAsync(bookId);

        public async Task RegisterLoanAsync(Book book, User user)
        {
            var loan = new Loan(user, book, DateTime.Now);
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(User user, int bookId)
        {
            var book = await FindBookAsync(bookId);
            var loan = user.Loans.FirstOrDefault(l => l.Book == book) 
                       ?? throw new InvalidOperationException("This user is not holding this book.");
            
            _context.Loans.Remove(loan);
        }

    }
}
