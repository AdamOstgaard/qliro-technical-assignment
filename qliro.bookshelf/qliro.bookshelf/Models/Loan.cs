using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace qliro.bookshelf.Models
{
    public class Loan
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public string UserID { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTimeOffset LoanTime { get; set; }
        public TimeSpan ValidFor { get; set; }

        private static readonly TimeSpan DefaultValidityTime = TimeSpan.FromDays(7);

        public Loan()
        {
            
        }

        public Loan(User user, Book book, DateTimeOffset loanTime, TimeSpan validFor)
        {
            ValidFor = validFor;
            LoanTime = loanTime;
            User = user;
            Book = book;
        }

        public Loan(User user, Book book, DateTimeOffset loanTime)
        {
            ValidFor = DefaultValidityTime;
            LoanTime = loanTime;
            User = user;
            Book = book;
        }
    }
}
