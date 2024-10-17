using System.Globalization;
using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Homies.Data.Models;
using static Homies.Constants.ApplicationConstants;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private HomiesDbContext _context;

        public EventController(HomiesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<EventIndexViewModel> events = await _context
                .Events
                .AsNoTracking()
                .Select(e => new EventIndexViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organizer.UserName
                }).ToListAsync();

            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //We need to check if the event is existing in the DB
            Event? e = await _context.Events
                .Include(e => e.Type)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (e == null)
            {
                return RedirectToAction(nameof(All));
            }

            EventDetailsViewModel model = new EventDetailsViewModel()
            {
                Id = id,
                Name = e.Name,
                Description = e.Description,
                Start = e.Start.ToString(DateTimeFormat),
                End = e.End.ToString(DateTimeFormat),
                Organiser = e.Organizer.UserName,
                CreatedOn = e.CreatedOn.ToString(DateTimeFormat),
                Type = e.Type.Name
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            EventInputViewModel model = new EventInputViewModel();
            model.Types = await GetEventTypes();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventInputViewModel inputViewModel)
        {
            if (ModelState.IsValid == false)
            {
                inputViewModel.Types = await GetEventTypes();
                return View(inputViewModel);
            }

            var (startDate, endDate, isDateValid) = await DateValidation(inputViewModel);

            if (isDateValid == false)
            {
                inputViewModel.Types = await GetEventTypes();
                return View(inputViewModel);
            }

            Event newEvent = new Event()
            {
                Name = inputViewModel.Name,
                Description = inputViewModel.Description,
                CreatedOn = DateTime.Now,
                Start = startDate,
                End = endDate,
                TypeId = inputViewModel.TypeId,
                OrganizerId = GetUserId()
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = GetUserId();

            Event? e = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == userId);

            if (e == null)
            {
                return RedirectToAction(nameof(All));
            }


            EventInputViewModel model = new EventInputViewModel()
            {
                Name = e.Name,
                Description = e.Description,
                Start = e.Start.ToString(DateTimeFormat),
                End = e.End.ToString(DateTimeFormat),
                TypeId = e.TypeId,
                Types = await GetEventTypes()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventInputViewModel inputModel, int id)
        {
            string userId = GetUserId();

            Event? e = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == userId);

            if (e == null)
            {
                return RedirectToAction(nameof(All));
            }

            if (ModelState.IsValid == false)
            {
                inputModel.Types = await GetEventTypes();
                return View(inputModel);
            }

            var (startDate, endDate, isDateValid) = await DateValidation(inputModel);

            if (isDateValid == false)
            {
                inputModel.Types = await GetEventTypes();
                return View(inputModel);
            }

            e.Name = inputModel.Name;
            e.Description = inputModel.Description;
            e.CreatedOn = DateTime.Now;
            e.Start = startDate;
            e.End = endDate;
            e.TypeId = inputModel.TypeId;
            e.OrganizerId = userId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            IEnumerable<JoinedEventsViewModel> eventsParticipants = await _context
                .EventsParticipants
                .Where(ep => ep.HelperId == userId)
                .Include(ep => ep.Event)
                .Include(ep => ep.Helper)
                .AsNoTracking()
                .Select(ep => new JoinedEventsViewModel()
                {
                    Id = ep.EventId,
                    Name = ep.Event.Name,
                    Start = ep.Event.Start.ToString(DateTimeFormat),
                    Type = ep.Event.Type.Name,
                    Organiser = ep.Event.Organizer.UserName
                }).ToListAsync();

            return View(eventsParticipants);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            //First we need to check if this event is existing in the Db
            Event? e = await _context
                .Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (e == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Next we need to check if this event is already added by the user in is joined events collection
            string userId = GetUserId();

            EventParticipant? ep = await _context
                .EventsParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == id && ep.HelperId == userId);

            if (ep != null)
            {
                return RedirectToAction(nameof(All));
            }

            //If the event doesn't exist in the user collection of joined events we add it
            await _context.EventsParticipants.AddAsync(new EventParticipant()
            {
                EventId = id,
                HelperId = userId,
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            //First we need to check if this event is existing in the Db
            Event? e = await _context
                .Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (e == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Next we need to check if this event is already added by the user in is joined events collection
            string userId = GetUserId();

            EventParticipant? ep = await _context
                .EventsParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == id && ep.HelperId == userId);

            if (ep == null)
            {
                return RedirectToAction(nameof(All));
            }

            //If the event exist in the user collection of joined events we remove it
            _context.EventsParticipants.Remove(ep);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }

        private async Task<(DateTime startDate, DateTime endDate, bool isDateValid)> DateValidation(EventInputViewModel inputViewModel)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            if (string.IsNullOrEmpty(inputViewModel.Start) || string.IsNullOrEmpty(inputViewModel.End))
            {
                ModelState.AddModelError(nameof(inputViewModel.Start), StartDateIsRequiredMessage);
                ModelState.AddModelError(nameof(inputViewModel.End), EndDateIsRequiredMessage);
                return (startDate, endDate, false);
            }

            if (DateTime.TryParseExact(inputViewModel.Start, DateTimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate) == false)
            {
                ModelState.AddModelError(nameof(inputViewModel.Start), DateTimeErrorMessage);
                return (startDate, endDate, false);
            }

            if (DateTime.TryParseExact(inputViewModel.End, DateTimeFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out endDate) == false)
            {
                ModelState.AddModelError(nameof(inputViewModel.End), DateTimeErrorMessage);
                return (startDate, endDate, false);
            }

            if (startDate > endDate)
            {
                ModelState.AddModelError(nameof(inputViewModel.End), EndDateIsBeforeStartDateMessage);
                return (startDate, endDate, false);
            }

            if (startDate == endDate)
            {
                ModelState.AddModelError(nameof(inputViewModel.Start), StartAndEndDateAreTheSameMessage);
                return (startDate, endDate, false);
            }

            return (startDate, endDate, true);
        }

        private async Task<IEnumerable<TypeSelectList>> GetEventTypes()
        {
            IEnumerable<TypeSelectList> types = await _context
                .Types
                .Select(t => new TypeSelectList()
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToListAsync();

            return types;
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }
    }
}
