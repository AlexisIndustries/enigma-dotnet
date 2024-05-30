using EnigmaDotnet.Data;
using EnigmaDotnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnigmaDotnet.Controllers;

public class AuthorController : Controller
{
    private ApplicationDbContext _applicationDbContext;

    public AuthorController(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    public IActionResult CreateAuthor()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateAuthor([Bind("Name,Biography")] Author author)
    {
        if (ModelState.IsValid)
        {
            _applicationDbContext.Authors.Add(author);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index", "Home");
    }
}