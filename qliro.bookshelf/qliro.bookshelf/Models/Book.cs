using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Qliro.BookShelf.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        
        /// <summary>
        /// The loaning user if there is one, otherwise null
        /// </summary>
        public Loan Loan { get; set; }

        public Book()
        {
            
        }

        public Book(string summary, string author, string title)
        {
            Summary = summary;
            Author = author;
            Title = title;
        }
    }
}
