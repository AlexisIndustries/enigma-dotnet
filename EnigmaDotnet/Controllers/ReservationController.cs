using System.Net;
using System.Security.Claims;
using EnigmaDotnet.Data;
using EnigmaDotnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnigmaDotnet.Controllers;

public class ReservationsController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    private UserManager<User> _userManager;

    public ReservationsController(ApplicationDbContext applicationDbContext, UserManager<User> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
    }

    // GET: Reservations/Reserve/5
    public ActionResult Reserve(string id)
    {
        if (id == null || _applicationDbContext.Books.Find(Guid.Parse(id)).Quantity <= 0)
        {
            return RedirectToAction("Index", "Home");
        }

        var book = _applicationDbContext.Books.Find(Guid.Parse(id));
        if (book == null || book.Quantity <= 0)
        {
            return NotFound();
        }
        var reservation = new Reservation
        {
            BookId = Guid.Parse(id)
        };
        return View(reservation);
    }

    // POST: Reservations/Reserve
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public ActionResult Reserve(Reservation reservation)
    {
        reservation.ReservationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        reservation.ReturnDate = DateTime.SpecifyKind(reservation.ReturnDate, DateTimeKind.Utc);
        reservation.Status = ReservationStatus.Requested;
        reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (ModelState.IsValid)
        {
            _applicationDbContext.Reservations.Add(reservation);
            _applicationDbContext.SaveChanges();
            var book = _applicationDbContext.Books.Find(reservation.BookId);
            book.Quantity--;
            _applicationDbContext.Books.Update(book);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(reservation);
    }

    // GET: Reservations/ViewerReservations
    [Authorize(Roles = "Worker, Admin")]
    public ActionResult WorkerReservations()
    {
        var reservations = _applicationDbContext.Reservations.Include(b => b.Book).Include(u => u.User).ToList();
        return View(reservations);
    }
    
    public ActionResult Edit(string id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        Reservation reservation = _applicationDbContext.Reservations.Include(b => b.Book).Include(b => b.User)
            .First(a => a.ReservationId == Guid.Parse(id));
        if (reservation == null)
        {
            return NotFound();
        }
        ViewBag.Status = new SelectList(Enum.GetValues(typeof(ReservationStatus)));
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind("ReservationId,BookId,UserId,ReservationDate,ReturnDate,Status")] Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            _applicationDbContext.Update(reservation);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Status = new SelectList(Enum.GetValues(typeof(ReservationStatus)));
        return View(reservation);
    }

    // GET: Reservations/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Reservation reservation = _applicationDbContext.Reservations.Find(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return View(reservation);
    }

    // POST: Reservations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(string id)
    {
        Reservation reservation = _applicationDbContext.Reservations.Find(Guid.Parse(id));
        if (reservation != null)
        {
            Book book = _applicationDbContext.Books.Find(reservation.BookId);
            if (book != null)
            {
                // Increase the book quantity when a reservation is canceled
                book.Quantity++;
            }

            _applicationDbContext.Reservations.Remove(reservation);
            _applicationDbContext.SaveChanges();
        }
        return RedirectToAction("Index", "Home");
    }
    
}