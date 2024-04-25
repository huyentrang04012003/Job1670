using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobWebsite.Data;
using JobWebsite.Models;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobWebsite.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CandidatesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: Candidates
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Candidates.ToListAsync());
        //}

        public async Task<IActionResult> ViewJobList(string? keyword)
        {
            var jobs = from j in _context.Jobs select j;
            if (!string.IsNullOrEmpty(keyword))
            {
                jobs = jobs.Where(j => j.Title.Replace(" ", "").Contains(keyword.Replace(" ", ""))
                        || j.Description.Replace(" ", "").Contains(keyword.Replace(" ", ""))
                        || j.Salary.ToString().Contains(keyword.Replace(" ", ""))
                        || j.Location.Replace(" ", "").Contains(keyword.Replace(" ", "")));
            }
            ViewBag.Keyword = keyword;
            return View(await jobs.ToListAsync());
        }


        public async Task<IActionResult> CreateCV(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var candidate = _context.Candidates.FirstOrDefault(c => c.User!.Id == user!.Id);
            var job = await _context.Jobs.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            if (candidate == null || job == null) 
            {
                return NotFound();
            }

            return View(new CV { Candidate = candidate, Job = job });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyJob(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var candidate = _context.Candidates.FirstOrDefault(c => c.User!.Id == user!.Id);
            var job = await _context.Jobs.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            if (candidate == null || job == null)
            {
                return NotFound();
            }

            _context.CVs.Add(new CV
            {
                Candidate = candidate,
                Job = job
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewJobList", "Candidates");
        }

        public async Task<IActionResult> ViewProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var candidate = _context.Candidates.FirstOrDefault(c => c.User!.Id == user!.Id);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }
      
        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(int id, Candidate candidate)
        {
            if (id != candidate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    candidate.Avatar = UploadAvatarImage(candidate);
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(ViewProfile));
        }
        

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }

        private string UploadAvatarImage(Candidate candidate)
        {
            string? imageFileName = null;
            if (candidate.AvatarImg != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                imageFileName = Guid.NewGuid().ToString() + "_" + candidate.AvatarImg.FileName;
                string filePath = Path.Combine(uploadsFolder, imageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    candidate.AvatarImg.CopyTo(fileStream);
                }
            }
            return imageFileName!;
        }
    }
}
