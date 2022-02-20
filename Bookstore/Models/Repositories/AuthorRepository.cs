using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorRepository : IBookRepository<Author>
    {
        IList<Author> authors;
        public AuthorRepository()
        {
            authors = new List<Author>()
              {
                  new Author { Id = 1 , FullNName = "Ali Elsaeed"},
                  new Author { Id = 2 , FullNName = "Ahmed Elsaeed"},
                  new Author { Id = 3 , FullNName = "Mohammed Elsaeed"}
              };
        }

        public void Add(Author entity)
        {
            entity.Id = authors.Max(a => a.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(b => b.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string term)
        {
            return authors.Where(a => a.FullNName.Contains(term)).ToList();
        }

        public void Update(int id, Author entity)
        {
            var author = Find(id);
            author.FullNName = entity.FullNName;

        }

        
    }
}
