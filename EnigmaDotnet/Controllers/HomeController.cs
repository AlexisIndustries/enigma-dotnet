using System.Diagnostics;
using EnigmaDotnet.Data;
using Microsoft.AspNetCore.Mvc;
using EnigmaDotnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EnigmaDotnet.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _applicationDbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _applicationDbContext = applicationDbContext;
    }

    public IActionResult Index()
    {
        var books = _applicationDbContext.Books.Include(a => a.Author).ToList();
        return View(books);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}