using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using WebAppPart1.Context;
using WebAppPart1.Models;

namespace WebAppPart1.Controllers
{
    // The controller class for handling the Home page actions
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        //Fetches bookings, venues, and events from the database independently
        public async Task<IActionResult> Index() 
        {
            var bookings = await _context.Bookings.ToListAsync();
            var venues = await _context.Venues.ToListAsync();
            var events = await _context.Events.ToListAsync();

            // ViewModel is created to pass data to the view
            var viewModel = new HomeViewModel
            {
                Bookings = bookings,
                Venues = venues,
                Events = events
            };

            // Return the view with the populated ViewModel
            return View(viewModel);
        }

        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .ToListAsync();

            return View(bookings); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}