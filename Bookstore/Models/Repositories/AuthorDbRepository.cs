using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorDbRepository : IBookRepository<Author>
    {
        BookStoreDbContext Db;
        public AuthorDbRepository(BookStoreDbContext _Db)
        {
            Db = _Db;
        }

        public void Add(Author entity)
        {
           // entity.Id = Db.Authors.Max(a => a.Id) + 1;
            Db.Authors.Add(entity);
            Db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);
            Db.Authors.Remove(author);
            Db.SaveChanges();
        }

        public Author Find(int id)
        {
            var author = Db.Authors.SingleOrDefault(b => b.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return Db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            return Db.Authors.Where(a => a.FullNName.Contains(term)).ToList();
        }

        public void Update(int id, Author entity)
        {
            Db.Authors.Update(entity);
            Db.SaveChanges();
                
        }
    }
}
