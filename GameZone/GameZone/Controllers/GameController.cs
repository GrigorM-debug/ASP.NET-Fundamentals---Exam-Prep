using System.Globalization;
using GameZone.Data;
using GameZone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static GameZone.Constants.ApplicationConstants;

namespace GameZone.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameZoneDbContext _context;

        public GameController(GameZoneDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> All()
        {
            ICollection<GameIndexViewModel> games = await _context
                .Games
                .Where(g => g.IsDeleted == false)
                .Select(g => new GameIndexViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Genre = g.Genre.Name,
                    ImageUrl = g.ImageUrl,
                    Publisher = g.Publisher.UserName ?? string.Empty,
                    ReleasedOn = g.ReleasedOn.ToString(DateFormat)
                })
                .ToListAsync();

            return View(games);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            GameInputViewModel inputModel = new()
            {
                Genres = await GetGenreList()
            };

            // Return the input model to the view
            return View(inputModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(GameInputViewModel input)
        {
            if (ModelState.IsValid == false)
            {
                input.Genres = await GetGenreList();

                return View(input); 
            }


            if (DateTime.TryParseExact(input.ReleasedOn, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime releasedOnDateTime) == false)
            {

                input.Genres = await GetGenreList();

                ModelState.AddModelError(nameof(input.ReleasedOn), InvalidDateMessage);

                return View(input);
            }

            Game newGame = new Game()
            {
                Title = input.Title,
                Description = input.Description,
                GenreId = input.GenreId,
                ImageUrl = input.ImageUrl,
                ReleasedOn = releasedOnDateTime,
                PublisherId = GetUserId(),
            };

            await _context.Games.AddAsync(newGame);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Game? game = await _context
                .Games
                .Include(g => g.Genre) // Ensure Genre is loaded
                .Include(g => g.Publisher) // Ensure Publisher is loaded
                .FirstOrDefaultAsync(g => g.Id == id); // Fetch game by ID

            if (game == null || game.IsDeleted)
            {
                return RedirectToAction(nameof(All));
            }

            GameDetailsViewModel detailsViewModel = new GameDetailsViewModel()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Genre = game.Genre.Name,
                ImageUrl = game.ImageUrl,
                Publisher = game.Publisher.UserName ?? string.Empty,
                ReleasedOn = game.ReleasedOn.ToString(DateFormat),
            };

            return View(detailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = GetUserId();

            Game? game = await _context
                .Games
                .AsNoTracking()
                .Include(g => g.GamersGames)
                .Include(game => game.Publisher)
                .FirstOrDefaultAsync(g=>g.Id == id && g.PublisherId == userId);

            if (game == null || game.IsDeleted)
            {
                return RedirectToAction(nameof(Details));
            }

            GameDeleteViewModel deleteViewModel = new GameDeleteViewModel()
            {
                Id = game.Id,
                Title = game.Title,
                Publisher = game.Publisher.UserName
            };

            return View(deleteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = GetUserId();

            Game? game = await _context
                .Games
                .FirstOrDefaultAsync(g => g.Id == id && g.PublisherId == userId);

            if (game == null || game.IsDeleted)
            {
                return RedirectToAction(nameof(DeleteConfirmed));
            }

            game.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string useId = GetUserId();

            Game? game = await _context
                .Games
                .FirstOrDefaultAsync(g => g.Id == id && g.PublisherId == useId);

            if (game == null || game.IsDeleted)
            {
                return RedirectToAction(nameof(Details));
            }

            GameInputViewModel inputViewModel = new GameInputViewModel
            {
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                ReleasedOn = game.ReleasedOn.ToString(DateFormat),
                GenreId = game.GenreId,
                Genres = await GetGenreList()
            };

            return View(inputViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameInputViewModel inputViewModel, int id)
        {
            string useId = GetUserId();

            Game? game = await _context
                .Games
                .FirstOrDefaultAsync(g => g.Id == id && g.PublisherId == useId);

            if (game == null || game.IsDeleted)
            {
                inputViewModel.Genres = await GetGenreList();

                return RedirectToAction(nameof(Details));
            }

            if (ModelState.IsValid == false)
            {
                inputViewModel.Genres = await GetGenreList();

                return View(inputViewModel);
            }

            if (DateTime.TryParseExact(inputViewModel.ReleasedOn, DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime releasedOnDateTime) == false)
            {
                ModelState.AddModelError(nameof(inputViewModel.ReleasedOn), InvalidDateMessage);

                inputViewModel.Genres = await GetGenreList();
                return View(inputViewModel);
            }

            game.Title = inputViewModel.Title;
            game.Description = inputViewModel.Description;
            game.GenreId = inputViewModel.GenreId;
            game.ImageUrl = inputViewModel.ImageUrl;
            game.ReleasedOn = releasedOnDateTime;
            game.PublisherId = GetUserId();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details));
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string userId = GetUserId();

            ICollection<GameIndexViewModel> indexViewModels =
                await _context
                    .GamersGames
                    .Include(gg => gg.Game)
                    .ThenInclude(g => g.Genre)
                    .Include(gg => gg.Gamer)
                    .Where(gg => gg.GamerId == userId && gg.IsDeleted == false)
                    .Select(gg => new GameIndexViewModel
                    {
                        Id = gg.GameId,
                        Title = gg.Game.Title,
                        ImageUrl = gg.Game.ImageUrl,
                        Genre = gg.Game.Genre.Name,
                        ReleasedOn = gg.Game.ReleasedOn.ToString(DateFormat),
                        Publisher = gg.Game.Publisher.UserName
                    })
                    .AsNoTracking()
                    .ToListAsync();

            return View(indexViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            Game? game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            GamerGame? gamerGame = await _context.GamersGames
                .FirstOrDefaultAsync(gg => gg.GameId == game.Id && gg.GamerId == userId);


            if (gamerGame != null)
            {
                if (!gamerGame.IsDeleted)
                {
                    return RedirectToAction(nameof(All));
                }

                gamerGame.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.GamersGames.AddAsync(new GamerGame()
                {
                    GameId = game.Id,
                    GamerId = userId
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyZone));
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            Game? game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return RedirectToAction(nameof(All));
            }

            string userId = GetUserId();

            GamerGame? gamerGame = await _context.GamersGames.FirstOrDefaultAsync(gg => gg.GameId == game.Id && gg.GamerId == userId);


            if (gamerGame == null || gamerGame.IsDeleted)
            {
                return RedirectToAction(nameof(All));
            }

            gamerGame.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyZone));
        }

        private string GetUserId()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return userId;
        }

        private async Task<ICollection<GenreSelectList>> GetGenreList()
        {
            ICollection<GenreSelectList> genres = await _context
                .Genres
                .AsNoTracking()
                .Select(g => new GenreSelectList
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            //ICollection<Genre> genres = await _context.Genres.ToListAsync();

            return genres;
        }
    }
}
