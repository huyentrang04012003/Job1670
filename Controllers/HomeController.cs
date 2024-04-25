using System.Diagnostics;
using JobWebsite.Data;
using JobWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ManageJobs()
        {
            return RedirectToAction("Index", "Jobs");
        }

        public async Task<IActionResult> ManageCVs()
        {
            var jobs = _context.Jobs;
            var jobIdList = jobs.Select(job => job.Id);
            var cvs = _context.CVs.Where(cv => jobIdList.Contains(cv.Job!.Id));

            //return RedirectToAction("ManageCVs", "Home");
            return View(await cvs.ToListAsync());
        }

        public async Task<IActionResult> ViewCVDetails(int? id)
        {
            var cv = await _context.CVs.Include(j => j.Job).Include(j =>j.Candidate)
                .FirstOrDefaultAsync(cv => cv.Id == id);
            //ViewBag.CV = cv;
            return View(cv);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
