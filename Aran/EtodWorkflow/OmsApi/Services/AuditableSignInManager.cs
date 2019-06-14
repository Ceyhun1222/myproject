using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OmsApi.Data;
using OmsApi.Entity;
using OmsApi.Interfaces;

namespace OmsApi.Services
{
    public class AuditableSignInManager : ISignInManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuditableSignInManager> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuditableSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
            ILogger<AuditableSignInManager> logger, ApplicationDbContext dbContext)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _userManager = userManager;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _db = dbContext;
        }

        public async Task<ApplicationUser> SignIn(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            UserAudit auditRecord;
            var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var isValid = true;
            if (user != null)
            {
                isValid = await _userManager.CheckPasswordAsync(user, password);
                if (isValid)
                    isValid = user.Status == Status.Accepted && !user.Disabled;
                _logger.LogInformation(isValid
                    ? $"User ({user.UserName}) signed in"
                    : $"User ({user.UserName}) failed the sign in");
                auditRecord = UserAudit.CreateAuditEvent(user.Id, isValid ? UserAuditEventType.Login : UserAuditEventType.FailedLogin, ip);
                user = isValid ? user : null;
            }
            else
            {
                _logger.LogInformation("Unknown user tried to sign in");
                auditRecord = UserAudit.CreateAuditEvent(0, UserAuditEventType.FailedLogin, ip);
            }
            _db.UserAuditEvents.Add(auditRecord);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task SignOutAsync()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(_contextAccessor.HttpContext.User));

            if (user != null)
            {
                var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                var auditRecord = UserAudit.CreateAuditEvent(user.Id, UserAuditEventType.LogOut, ip);
                _db.UserAuditEvents.Add(auditRecord);
                await _db.SaveChangesAsync();
            }
        }
    }
}
