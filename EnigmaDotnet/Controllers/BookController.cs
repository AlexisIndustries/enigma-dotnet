using EnigmaDotnet.Data;
using EnigmaDotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnigmaDotnet.Controllers;

public class BookController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public BookController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    [HttpGet]
    public IActionResult CreateBook()
    {
        var authors = _applicationDbContext.Authors.ToList().Select(a => new {a.AuthorId, a.Name}).ToList();
        if (authors.Count() != 0)
        {
            ViewBag.Authors = authors;
        }
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateBook([Bind("Title,AuthorId,Publisher,ISBN,Year,Quantity")] Book book, IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image != null)
            {
                using var binaryReader = new BinaryReader(image.OpenReadStream());
                book.Image = binaryReader.ReadBytes((int)image.Length);
            }
            _applicationDbContext.Books.Add(book);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Authors = _applicationDbContext.Authors.Select(a => new { a.AuthorId, a.Name }).ToList();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult BookDetails(string id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Book book = _applicationDbContext.Books.Include(a=> a.Author).ToList().First(b => b.BookId == Guid.Parse(id));
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }
    
    public IActionResult BookEdit(string id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Book book = _applicationDbContext.Books.Include(a=> a.Author).ToList().First(b => b.BookId == Guid.Parse(id));
        if (book == null)
        {
            return NotFound();
        }
        ViewBag.AuthorId = new SelectList(_applicationDbContext.Authors.ToList(), "AuthorId", "Name", book.AuthorId);
        return View(book);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult BookEdit([Bind("BookId,Title,AuthorId,Publisher,ISBN,Year,ImageUrl, Quantity")] Book book, IFormFile image)
    {
        byte[] oldImage = _applicationDbContext.Books.Include(a=> a.Author).ToList().First(b => b.BookId == book.BookId).Image;
        if (ModelState.IsValid)
        {
            if (image != null)
            {
                using var binaryReader = new BinaryReader(image.OpenReadStream());
                book.Image = binaryReader.ReadBytes((int)image.Length);
            }
            else
            {
                book.Image = oldImage;
            }

            _applicationDbContext.Update(book);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        ViewBag.AuthorId = new SelectList(_applicationDbContext.Authors, "AuthorId", "Name", book.AuthorId);
        return View(book);
    }

    [HttpGet]
    public IActionResult BookDelete(string id)
    {
        Book book = _applicationDbContext.Books.Include(a => a.Author).ToList().First(b => b.BookId == Guid.Parse(id));
        _applicationDbContext.Books.Remove(book);
        _applicationDbContext.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

}