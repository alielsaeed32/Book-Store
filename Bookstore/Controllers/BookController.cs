using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository<Book> BookRepo;
        private readonly IBookRepository<Author> AuthorRepo;
        private readonly IHostingEnvironment hosting;
        public BookController(IBookRepository<Book> BookRepo ,
            IBookRepository<Author> AuthorRepo , 
            IHostingEnvironment hosting)
        {
            this.BookRepo = BookRepo;
            this.AuthorRepo = AuthorRepo;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = BookRepo.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = BookRepo.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if(ModelState.IsValid)
            {
                //if(model.File != null)
                //{
                //    string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                //    FileName = model.File.FileName;
                //    string fullName = Path.Combine(uploads, FileName);
                //    model.File.CopyTo(new FileStream(fullName, FileMode.Create));
                //}
                try
                {
                    string FileName = UploadFile(model.File) ?? string.Empty;
                    if (model.AuthorId == -1)
                    {
                        ViewBag.message = "Please Select an Author from the List!";
                       
                        return View(getAllBooks());
                    }
                    var author = AuthorRepo.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        imageURL = FileName
                    };
                    BookRepo.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
           else
            {
                ModelState.AddModelError("", "You Have To Fill All Required Fields");
                return View(getAllBooks());
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = BookRepo.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var model = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId ,
                Authors = AuthorRepo.List().ToList(),
                ImageURL = book.imageURL
            };
            return View(model);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model )
        {
            try
            {
                string FileName = UploadFile(model.File, model.ImageURL);
                //if (model.File != null)
                //{
                //    string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                //    FileName = model.File.FileName;
                //    string fullName = Path.Combine(uploads, FileName);

                //    //Delete old
                //    string oldFileName = model.ImageURL;//BookRepo.Find(model.BookId).imageURL;
                //    string fullOldPath = Path.Combine(uploads, oldFileName);

                //    if (fullName != fullOldPath)
                //    {
                //        System.IO.File.Delete(fullOldPath);
                //        model.File.CopyTo(new FileStream(fullName, FileMode.Create));
                //    }

                   
                //}
                var author = AuthorRepo.Find(model.AuthorId);
                Book book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = author,
                    imageURL = FileName
                };
                BookRepo.Update(model.BookId,book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = BookRepo.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                BookRepo.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = AuthorRepo.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullNName = "--- Please Select an Author" });
            return authors;
        }
        BookAuthorViewModel getAllBooks()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;
        }
        string UploadFile (IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                string fullName = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullName, FileMode.Create));
                return file.FileName;
            }
            else
                return null;
        }

        string UploadFile(IFormFile file, string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                string newPath = Path.Combine(uploads, file.FileName);

                
                string oldPath = imageUrl;
               
                if (oldPath != newPath)
                {
                    System.IO.File.Delete(oldPath); //Delete old image
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }
                return file.FileName;
            }
            return imageUrl;
        }

        public ActionResult Search(string term)
        {
            var reslt = BookRepo.Search(term);
            return View("Index", reslt);
        }
    }
}
