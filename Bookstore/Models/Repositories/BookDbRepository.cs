using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class BookDbRepository : IBookRepository<Book>
    {
        BookStoreDbContext Db;
        public BookDbRepository(BookStoreDbContext _Db)
        {
            Db = _Db;
        }
        public void Add(Book entity)
        {
           // entity.Id = Db.books.Max(b => b.Id) + 1;
            Db.books.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Db.books.Include(a=>a.Author).SingleOrDefault(b => b.Id == id);
            Db.books.Remove(book);
            Db.SaveChanges();
        }

        public Book Find(int id)
        {
            var book = Db.books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return Db.books.Include(a => a.Author).ToList();
        }

        public void Update(int id, Book newEntity)
        {
            Db.Update(newEntity);
            Db.SaveChanges();
        }

        public List<Book> Search(string term)
        {
            var result = Db.books.Include(a => a.Author)
                .Where(b => b.Title.Contains(term)
                || b.Description.Contains(term)
                || b.Author.FullNName.Contains(term)).ToList();
            return result;
        }
    }
}
