using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppPart1.Context;
using WebAppPart1.Models;


namespace WebAppPart1.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (Request.Method == "GET" && Request.Query.ContainsKey("searchString") && string.IsNullOrWhiteSpace(searchString))
            {
                ViewData["SearchError"] = "Please enter a value to search.";
            }

            //BookingID or EventName 
            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchString) ||
                    b.Event.EventName.Contains(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            // Validate Event and Venue existence
            bool eventExists = await _context.Events.AnyAsync(e => e.EventId == booking.EventId);
            bool venueExists = await _context.Venues.AnyAsync(v => v.VenueId == booking.VenueId);

            if (!eventExists || !venueExists)
            {
                ModelState.AddModelError("", "Event ID or Venue ID does not exist.");
                return View(booking);
            }
            
            // Check if the venue is already booked on the selected date
            bool isVenueBooked = await _context.Bookings
                .AnyAsync(b => b.VenueId == booking.VenueId &&
                               EF.Functions.DateDiffDay(b.BookingDate, booking.BookingDate) == 0);

            if (isVenueBooked)
            {
                ModelState.AddModelError("", "This venue is already booked on the selected date. Please choose a different date.");
                return View(booking);
            }

            // If ModelState is valid, save the booking to the database
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. The venue may already be booked on that date.");
                    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                }
            }

            return View(booking);  
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Checks if the venue is already booked on the selected date
                    bool isVenueBooked = await _context.Bookings
                        .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate == booking.BookingDate && b.BookingId != booking.BookingId);

                    if (isVenueBooked)
                    {
                        ModelState.AddModelError("", "This venue is already booked on the selected date. Please choose another date or venue.");
                        return View(booking);
                    }

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. The venue may already be booked on that date.");
                    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                    return View(booking);
                }
            }

            return View(booking);
        }


        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
