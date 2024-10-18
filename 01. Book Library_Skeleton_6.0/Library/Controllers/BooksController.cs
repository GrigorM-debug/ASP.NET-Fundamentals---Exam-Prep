using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using NuGet.Frameworks;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<BookIndexViewModel> books = await _context
                .Books
                .AsNoTracking()
                .Select(b => new BookIndexViewModel()
                {
                    Id = b.Id,
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                    Author = b.Author,
                    Rating = b.Rating,
                    Category = b.Category.Name
                }).ToListAsync();

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            BookInputViewModel model = new BookInputViewModel();
            model.Categories = await GetCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookInputViewModel inputModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(inputModel);
            }

            Book newBook = new Book()
            {
                Title = inputModel.Title,
                Author = inputModel.Author,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl,
                Rating = inputModel.Rating,
                CategoryId = inputModel.CategoryId,
            };

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = GetUserId();

            IEnumerable<MyBooksViewModel> books = await _context
                .ApplicationUsersBooks
                .AsNoTracking()
                .Where(b => b.ApplicationUserId == userId && b.IsDeleted == false)
                .Select(b => new MyBooksViewModel()
                {
                    Id = b.BookId,
                    ImageUrl = b.Book.ImageUrl,
                    Title = b.Book.Title,
                    Author = b.Book.Author,
                    Rating = b.Book.Rating,
                    Category = b.Book.Category.Name,
                    Description = b.Book.Description
                })
                .ToListAsync();

            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int bookId)
        {
            string userId = GetUserId();

            //First we need to check if this book is existing
            Book? book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Then we must check if  already added or marked as deleted
            ApplicationUserBook? applicationUserBook = await _context
                .ApplicationUsersBooks
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.ApplicationUserId == userId);

            if (applicationUserBook != null)
            {
                if (applicationUserBook.IsDeleted == true)
                {
                    applicationUserBook.IsDeleted = false;
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Mine));
                }

                return RedirectToAction(nameof(All));
            }

            await _context.ApplicationUsersBooks.AddAsync(new ApplicationUserBook()
            {
                ApplicationUserId = userId,
                BookId = bookId
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            string userId = GetUserId();

            //First we need to check if this book is existing
            Book? book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return RedirectToAction(nameof(All));
            }

            //Then we must check if  already added or marked as deleted
            ApplicationUserBook? applicationUserBook = await _context
                .ApplicationUsersBooks
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.ApplicationUserId == userId);

            if (applicationUserBook == null && (applicationUserBook != null && applicationUserBook.IsDeleted == true))
            {
                return RedirectToAction(nameof(All));
            }

            applicationUserBook.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<CategorySelectList>> GetCategories()
        {
            IEnumerable<CategorySelectList> categories =
                await _context
                    .Categories
                    .AsNoTracking()
                    .Select(c => new CategorySelectList()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToListAsync();

            return categories;
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }
    }
}
