using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using qliro.bookshelf.Data;
using qliro.bookshelf.Models;
using qliro.bookshelf.Services;

namespace qliro.bookshelf.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookShelfController : ControllerBase
    {
        private readonly ILogger<BookShelfController> _logger;
        private readonly BookShelfService _bookShelfService;

        public BookShelfController(ILogger<BookShelfController> logger, BookShelfService bookShelfService)
        {
            _logger = logger;
            _bookShelfService = bookShelfService;
        }

        [HttpGet("/books")]
        public IEnumerable<Book> AllBooks()
            => _bookShelfService.GetAllBooks();

        [HttpPost("/loan/{bookId}")]
        public async Task Loan([FromRoute]int bookId)
        {
            var currentUserName = User.FindFirst(ClaimTypes.Name).Value;

            if (currentUserName == null)
            {
                throw new InvalidOperationException("User does not have a Name/ID");
            }

            var user = await _bookShelfService.FindUserAsync(currentUserName);
            var book = await _bookShelfService.FindBookAsync(bookId);

            if (book.Loan != null)
            {
                throw new InvalidOperationException($"Book is already loaned by {book.Loan.User.Name}.");
            }

            await _bookShelfService.RegisterLoanAsync(book, user);
        }

        [HttpPost("/return/{bookId}")]
        public async Task Return([FromRoute]int bookId)
        {
            var currentUserName = User.FindFirst(ClaimTypes.Name).Value;

            if (currentUserName == null)
            {
                throw new InvalidOperationException("User does not have a Name/ID");
            }

            var user = await _bookShelfService.FindUserAsync(currentUserName);

            try
            {
                await _bookShelfService.ReturnBookAsync(user, bookId);
            }
            catch (InvalidOperationException e)
            {
                BadRequest(e.Message);
            }
        }

        [HttpPost("/loans")]
        public async Task<IEnumerable<Loan>> GetLoans()
        {
            var currentUserName = User.FindFirst(ClaimTypes.Name).Value;

            var user = await _bookShelfService.FindUserAsync(currentUserName);
            return user.Loans;
        }
    }
}
