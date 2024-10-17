using Contacts.Data;
using Contacts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Contacts.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly ContactsDbContext _context;

        public ContactsController(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ContactIndexViewModel> contacts = await _context
                .Contacts
                .AsNoTracking()
                .Select(c => new ContactIndexViewModel()
                {
                    ContactId = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Website = c.Website
                }).ToListAsync();

            return View(contacts);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ContactInputViewModel model = new ContactInputViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContactInputViewModel inputViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(inputViewModel);
            }

            Contact newContact = new Contact
            {
                FirstName = inputViewModel.FirstName,
                LastName = inputViewModel.LastName,
                Email = inputViewModel.Email,
                PhoneNumber = inputViewModel.PhoneNumber,
                Address = inputViewModel.Address,
                Website = inputViewModel.Website,
            };

            await _context.Contacts.AddAsync(newContact);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return RedirectToAction(nameof(All));
            }

            ContactInputViewModel model = new ContactInputViewModel()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address,
                Website = contact.Website
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ContactInputViewModel inputModel, int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return View(inputModel);
            }

            if (ModelState.IsValid == false)
            {
                return View(inputModel);
            }

            contact.FirstName = inputModel.FirstName;
            contact.LastName = inputModel.LastName;
            contact.Email = inputModel.Email;
            contact.PhoneNumber = inputModel.PhoneNumber;
            contact.Address = inputModel.Address;
            contact.Website = inputModel.Website;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Team()
        {
            string userId = GetUserId();

            IEnumerable<ContactIndexViewModel> models = await _context
                .ApplicationUsersContacts
                .Where(ac => ac.ApplicationUserId == userId)
                .AsNoTracking()
                .Select(ac => new ContactIndexViewModel()
                {
                    ContactId = ac.ContactId,
                    FirstName = ac.Contact.FirstName,
                    LastName = ac.Contact.LastName,
                    Email = ac.Contact.Email,
                    PhoneNumber = ac.Contact.PhoneNumber,
                    Address = ac.Contact.Address,
                    Website = ac.Contact.Website,
                }).ToListAsync();

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> AddToTeam(int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            ApplicationUserContact? applicationUserContacts = await _context
                .ApplicationUsersContacts
                .FirstOrDefaultAsync(ac => ac.ContactId == id && ac.ApplicationUserId == userId);

            if (applicationUserContacts != null)
            {
                return RedirectToAction(nameof(All));
            }

           await _context.AddAsync(new ApplicationUserContact()
            {
                ContactId = id,
                ApplicationUserId = userId,
            });

           await _context.SaveChangesAsync();

           return RedirectToAction(nameof(Team));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromTeam(int id)
        {
            Contact? contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            ApplicationUserContact? applicationUserContacts = await _context
                .ApplicationUsersContacts
                .FirstOrDefaultAsync(ac => ac.ContactId == id && ac.ApplicationUserId == userId);

            if (applicationUserContacts == null)
            {
                return RedirectToAction(nameof(All));
            }

            _context.Remove(applicationUserContacts);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }
    }
}
