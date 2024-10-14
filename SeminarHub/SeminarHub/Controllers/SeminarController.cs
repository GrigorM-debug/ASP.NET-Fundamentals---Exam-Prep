using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeminarHub.Constants;
using SeminarHub.Data;
using SeminarHub.Models;
using static SeminarHub.Constants.ApplicationConstants;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext _context;

        public SeminarController(SeminarHubDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> All()
        {
            ICollection<SeminarIndexViewModel> seminars = await _context
                .Seminars
                .Where(s => s.IsDeleted == false)
                .Select(s => new SeminarIndexViewModel()
                {
                    Id = s.Id,
                    Lecturer = s.Lecturer,
                    Topic = s.Topic,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime.ToString(ApplicationConstants.DateFormat),
                    Organizer = s.Organizer.UserName
                }).ToListAsync();


            return View(seminars);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Seminar? seminar = await _context.Seminars
                .Include(s=> s.Category)
                .Include(s => s.Organizer)
                .FirstOrDefaultAsync(s=> s.Id == id && s.IsDeleted == false);

            if (seminar == null)
            {
                return RedirectToAction(nameof(All));
            }

            SeminarDetailsViewModel model = new SeminarDetailsViewModel()
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                Category = seminar.Category.Name,
                DateAndTime = seminar.DateAndTime.ToString(ApplicationConstants.DateFormat),
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                Organizer = seminar.Organizer.UserName,
                Duration = seminar.Duration
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            SeminarInputViewModel model = new SeminarInputViewModel
            {
                Categories = await GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarInputViewModel inputModel)
        {
            if (ModelState.IsValid == false)
            {
                inputModel.Categories = await GetCategories();
                return View(inputModel); 
            }

            if (DateTime.TryParseExact(inputModel.DateAndTime, ApplicationConstants.DateFormat, CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out DateTime dateTime) == false)
            {
                ModelState.AddModelError(nameof(inputModel.DateAndTime), DateFormatErrorMessage);
                inputModel.Categories = await GetCategories();
                return View(inputModel);
            }

            Seminar seminar = new Seminar
            {
                Topic = inputModel.Topic,
                Lecturer = inputModel.Lecturer,
                Details = inputModel.Details,
                DateAndTime = dateTime,
                Duration = inputModel.Duration,
                OrganizerId = GetUserId(),
                CategoryId = inputModel.CategoryId,
            };

            await _context.Seminars.AddAsync(seminar);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = GetUserId();

            Seminar? seminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false && s.OrganizerId == userId);

            if (seminar == null)
            {
                return RedirectToAction(nameof(Details));
            }


            SeminarDeleteViewModel model = new SeminarDeleteViewModel()
            {
                Id = seminar.Id,
                DateAndTime = seminar.DateAndTime,
                Topic = seminar.Topic
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = GetUserId();

            Seminar? seminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false && s.OrganizerId == userId);

            if (seminar == null)
            {
                return RedirectToAction(nameof(Details));
            }

            seminar.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = GetUserId();

            Seminar? seminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false && s.OrganizerId == userId);

            if (seminar == null)
            {
                return RedirectToAction(nameof(Details));
            }

            SeminarInputViewModel model = new SeminarInputViewModel
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime.ToString(ApplicationConstants.DateFormat),
                Duration = seminar.Duration,
                CategoryId = seminar.CategoryId,
                Categories = await GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarInputViewModel inputModel, int id)
        {
            string userId = GetUserId();

            Seminar? seminar = await _context
                .Seminars
                .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false && s.OrganizerId == userId);

            if (seminar == null)
            {
                inputModel.Categories = await GetCategories();
                return View(inputModel); ;
            }

            if (ModelState.IsValid == false)
            {
                inputModel.Categories = await GetCategories();
                return View(inputModel);
            }

            if (DateTime.TryParseExact(inputModel.DateAndTime, ApplicationConstants.DateFormat, CultureInfo.CurrentCulture, 
                    DateTimeStyles.None, out DateTime dateTime) == false)
            {
                ModelState.AddModelError(nameof(inputModel.DateAndTime), DateFormatErrorMessage);
                inputModel.Categories = await GetCategories();
                return View(inputModel);
            }

            seminar.Topic = inputModel.Topic;
            seminar.DateAndTime = dateTime;
            seminar.CategoryId = inputModel.CategoryId;
            seminar.Details = inputModel.Details;
            seminar.Duration = inputModel.Duration;
            seminar.Lecturer = inputModel.Lecturer;
            seminar.OrganizerId = GetUserId();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            ICollection<SeminarIndexViewModel> seminarParticipants =
                await _context
                    .SeminarsParticipants
                    .Include(sp => sp.Seminar)
                    .Include(sp => sp.Participant)
                    .Where(sp => sp.ParticipantId == userId && sp.IsDeleted == false)
                    .Select(sp => new SeminarIndexViewModel
                    {
                        Id = sp.SeminarId,
                        Topic = sp.Seminar.Topic,
                        Lecturer = sp.Seminar.Lecturer,
                        Category = sp.Seminar.Category.Name,
                        DateAndTime = sp.Seminar.DateAndTime.ToString(ApplicationConstants.DateFormat),
                        Organizer = sp.Seminar.Organizer.UserName,
                    })
                    .ToListAsync();

            return View(seminarParticipants);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            Seminar? seminar = await _context.Seminars.FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false);

            if (seminar == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            SeminarParticipant? seminarParticipant =
                await _context.SeminarsParticipants.FirstOrDefaultAsync(sp =>
                    sp.SeminarId == id && sp.ParticipantId == userId);

            if (seminarParticipant != null)
            {
                if (!seminarParticipant.IsDeleted)
                {
                    return RedirectToAction(nameof(All));
                }

                seminarParticipant.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.AddAsync(new SeminarParticipant()
                {
                    SeminarId = seminar.Id,
                    ParticipantId = userId
                });

                await _context.SaveChangesAsync();
            }

            

            return RedirectToAction(nameof(Joined));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            Seminar? seminar = await _context.Seminars.FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false);

            if (seminar == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            SeminarParticipant? seminarParticipant =
                await _context.SeminarsParticipants.FirstOrDefaultAsync(sp =>
                    sp.SeminarId == id && sp.ParticipantId == userId && sp.IsDeleted == false);

            if (seminarParticipant == null)
            {
                return RedirectToAction(nameof(All));
            }

            seminarParticipant.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }

        private async Task<ICollection<CategorySelectList>> GetCategories()
        {
            ICollection<CategorySelectList> categories = await _context
                .Categories
                .Select(c => new CategorySelectList()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();

            return categories;
        }
    }
}
