using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class BookRepository : IBookRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id=1 , 
                    Title = "C# programming" , 
                    Description = "No thing" ,
                    imageURL = "b1.jpg" ,
                    Author = new Author{ Id= 2} 
                    
                },
                  new Book
                {
                    Id=2 , 
                    Title = "java programming" , 
                    Description = "No thing" ,
                     imageURL = "b2.jpg" ,
                    Author = new Author() 
                   
                },
                    new Book
                {
                    Id=3 , 
                    Title = "Paython programming" , 
                    Description = "No thing" ,
                    imageURL = "b3.jpg" ,
                    Author = new Author() 
                    
                }
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(b => b.Title.Contains(term)).ToList();
        }

        public void Update(int id ,Book newEntity)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            book.Title = newEntity.Title;
            book.Description = newEntity.Description;
            book.Author = newEntity.Author;
            book.imageURL = newEntity.imageURL;
        }
    }
}
