using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qliro.BookShelf.Data;
using Qliro.BookShelf.Models;

namespace Qliro.BookShelf.Services
{
    public class BookShelfService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookShelfService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Book> GetAllBooks()
            => _dbContext.Books;

        public async Task<User> FindUserAsync(string userId)
            => await _dbContext.Users.FindAsync(userId);

        public async Task<Book> FindBookAsync(int bookId)
            => await _dbContext.Books.FindAsync(bookId);

        public async Task RegisterLoanAsync(Book book, User user)
        {
            var loan = new Loan(user, book, DateTime.Now);
            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(User user, int bookId)
        {
            var book = await FindBookAsync(bookId);
            var loan = user.Loans.FirstOrDefault(l => l.Book == book) 
                       ?? throw new InvalidOperationException("This user is not holding this book.");
            
            _dbContext.Loans.Remove(loan);
        }

    }
}
