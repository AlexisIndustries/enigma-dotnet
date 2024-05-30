using EnigmaDotnet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnigmaDotnet.Controllers;

public class SearchController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public SearchController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public ActionResult SearchResults(string searchString, string[] searchBy)
    {
        var books = _applicationDbContext.Books.Include(a => a.Author).ToList();

        if (!string.IsNullOrEmpty(searchString))
        {
            if (searchBy.Contains("Author"))
            {
                books = books.Where(b => b.Author.Name.Contains(searchString)).ToList();
            }
            if (searchBy.Contains("Title"))
            {
                books = books.Where(b => b.Title.Contains(searchString)).ToList();
            }
            if (searchBy.Contains("Year"))
            {
                books = books.Where(b => b.Year.ToString().Contains(searchString)).ToList();
            }
        }

        return View(books.ToList());
    }
}