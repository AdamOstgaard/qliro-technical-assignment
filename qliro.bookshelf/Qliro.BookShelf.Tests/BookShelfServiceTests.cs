using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Qliro.BookShelf.Data;
using Qliro.BookShelf.Models;
using Qliro.BookShelf.Services;
using Xunit;

namespace Qliro.BookShelf.Tests
{
    public class BookShelfServiceTests
    {
        [Fact]
        public async Task CanGetAllBooks()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            await using var context = new ApplicationDbContext(options);

            var testBooks = new List<Book>
            {
                new Book("foo", "bar", "test book") {ID = 1},
                new Book("bar", "foo", "book test") {ID = 2}
            };

            foreach (var book in testBooks)
            {
                context.Books.Add(book);
            }

            await context.SaveChangesAsync();

            var service = new BookShelfService(context);
            var allBooks = service.GetAllBooks().ToList();

            var correctLength = allBooks.Count == testBooks.Count;
            var containsAllElements = allBooks.All(b => testBooks.Contains(b));

            Assert.True(correctLength && containsAllElements);
        }

        [Fact]
        public async Task CanLoanBook()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            await using var context = new ApplicationDbContext(options);

            var userId = Guid.NewGuid().ToString();
            const int bookId = -1;

            context.Books.Add(new Book("foo", "bar", "test book"){ID = bookId});
            context.Users.Add(new User {UserName = "testUser", ID = userId});
            await context.SaveChangesAsync();

            var service = new BookShelfService(context);
            var user = await service.FindUserAsync(userId);
            var book = await service.FindBookAsync(bookId);

            await service.RegisterLoanAsync(book, user);

            var exists = await context.Loans.AnyAsync(l => l.Book == book && l.User == user);

            Assert.True(exists);
        }
    }
}
