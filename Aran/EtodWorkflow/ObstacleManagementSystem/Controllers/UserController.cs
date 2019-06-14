using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ObstacleManagementSystem.Data;
using ObstacleManagementSystem.Models;
using ObstacleManagementSystem.Services;
using ObstacleManagementSystem.ViewModels.UserViewModels;

namespace ObstacleManagementSystem.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Moderator))]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly ILogger _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ApplicationDbContext applicationDbContext,
            IStringLocalizer<UserController> stringLocalizer,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _context = applicationDbContext;
            _localizer = stringLocalizer;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            AddBreadcrumb(_localizer["Users"], "/User/Index");
            var user = await _userManager.GetUserAsync(User);
            var lst = await _userManager.Users.Except(new List<ApplicationUser>() { user }).ToListAsync();
            return View(lst);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            AddBreadcrumb(_localizer["Users"], "/User/Index");
            AddBreadcrumb(_localizer["Details"], "");
            if (id == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(applicationUser);
            var result = new UserWithRoleViewModel
            {
                User = applicationUser,
                Role = (Roles)Enum.Parse(typeof(Roles), roles[0])
            };
            return View(result);
        }

        public async Task<IActionResult> ChangeRole(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var lst = await _userManager.GetRolesAsync(user);
            if (!Enum.TryParse(typeof(Roles), lst[0], out var role))
                role = Roles.Client;

            var viewModel = new UserWithRoleViewModel()
            {
                User = user,
                Role = (Roles)role
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int? id, UserWithRoleViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

            }
            return View();
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,Fathername,Birthday,Gender,PassportNumber,IdNumber,IdFinNumber,PasswordChanged,DateRegistered,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Edit(long? id, [Bind("Firstname,Lastname,Fathername,Birthday,Gender,PasswordChanged,DateRegistered,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id == null || id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
