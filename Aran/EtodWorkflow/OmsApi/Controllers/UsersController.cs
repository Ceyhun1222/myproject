using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OmsApi.Data;
using OmsApi.Dto;
using OmsApi.Entity;
using OmsApi.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ILogger<UsersController> logger, IMapper mapper, 
            ApplicationDbContext dbContext)
        {

            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet]
        [SwaggerOperation("Gets all users")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get(DtoStatus status)
        {
            IQueryable<ApplicationUser> users = _userManager.Users;
            if (User?.Identity?.Name != null)
                users = users.Where(t => t.UserName != User.Identity.Name);
            if (status != DtoStatus.All)
            {
                Status st = (Status)status;
                if (status == DtoStatus.Pending)
                    users = users.Where(t => t.Status == st || t.Status == Status.New);
                else
                    users = users.Where(t => t.Status == st);
            }
            var lst = await users.OrderByDescending(t=>t.CreatedAt).ToListAsync();

           lst.ForEach(t =>
            {
                if (t.Status == Status.New)
                    t.Status = Status.Pending;
            });
            await _dbContext.SaveChangesAsync();
            var userDtoList = _mapper.Map<IList<UserDto>>(lst);
            return Ok(userDtoList);
        }

        //[Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("BasicInfos")]
        [SwaggerOperation("Gets all users' basic infos (Fullname and ID)")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserBasicInfoDto>>> GetBasicInfos()
        {
            IQueryable<ApplicationUser> users = _userManager.Users;
            if (User?.Identity?.Name != null)
                users = users.Where(t => t.UserName != User.Identity.Name);
            users = users.Where(t => t.Status == Status.Accepted);
            var lst = await users.OrderByDescending(t => t.CreatedAt).ToListAsync();
            var userDtoList = _mapper.Map<IList<UserBasicInfoDto>>(lst);
            return Ok(userDtoList);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("{id}")]
        [SwaggerOperation("Gets specified user")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return BadRequest(NotFound());
            var currUser = await GetUser(_userManager);
            if (!await _userManager.IsInRoleAsync(currUser, Roles.Admin.ToString()))
            {
                if (id != currUser.Id)
                    return Forbid();
            }
            var result = _mapper.Map<UserDto>(user);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Profile")]
        [SwaggerOperation("Gets current logged in user's profile")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> Profile()
        {

            var curruser = await GetUser(_userManager);
            return Ok(_mapper.Map<UserDto>(curruser));
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [Route("NotificationCount")]
        [HttpGet]
        [SwaggerOperation("Gets user sign-up notifications count")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetNotificationCount()
        {
            var count = await _userManager.Users.CountAsync(t => t.Status == Status.New);
            return Ok(count);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        [SwaggerOperation("User registration")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status409Conflict, "Username exists")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent, "Successfully created")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest, "Invalid model")]
        public async Task<IActionResult> Post(UserRegistration registrationData, [FromServices]ApplicationDbContext dbContext)
        {
            var user = await _userManager.FindByNameAsync(registrationData.UserName);
            if (user != default)
                return Conflict();

            try
            {
                user = _mapper.Map<ApplicationUser>(registrationData);
            }
            catch (System.Exception ex)
            {
                var k = ex.Message;
                throw;
            }
            
            user.Status = Status.Accepted;
            var identityResult = await _userManager.CreateAsync(user, registrationData.Password);

            if (!identityResult.Succeeded)
            {
                GetModelStateFor(identityResult);
                return BadRequest(ModelState);
            }

            var res = await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
            if (!res.Succeeded)
            {
                GetModelStateFor(res);
                return BadRequest(ModelState);
            }
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        [SwaggerOperation("Updates specified user")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(UserDto user)
        {
            try
            {
                var resUser = await _userManager.FindByNameAsync(user.UserName);
                if (resUser == null)
                    return BadRequest(NotFound());
                if (resUser.Id != user.Id)
                    return BadRequest(NotFound());
                var currUser = await GetUser(_userManager);
                if (!await _userManager.IsInRoleAsync(currUser, Roles.Admin.ToString()))
                {
                    if (resUser.Id != currUser.Id)
                        return Forbid();
                }
                resUser = _mapper.Map<UserDto, ApplicationUser>(user, resUser);                
                var res = await _userManager.UpdateAsync(resUser);
                if (!res.Succeeded)
                {
                    GetModelStateFor(res);
                    return BadRequest(ModelState);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Put  User error");
                throw;
            }
            return NoContent();
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id}")]
        [SwaggerOperation("Deletes specified user")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return BadRequest(NotFound());
            _logger.LogInformation($"User is {user.Firstname} {user.Lastname}");
            try
            {
                var res = await _userManager.DeleteAsync(user);
                if (!res.Succeeded)
                {
                    _logger.LogInformation("DeleteUser", res.Errors.FirstOrDefault().Description);
                    GetModelStateFor(res);
                    return BadRequest(ModelState);
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "DeleteUser");
                throw;
            }
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{userId}/{isAccepted}")]
        [SwaggerOperation("Accepts/declines user registration request")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Accept(long userId, bool isAccepted, [FromServices] ApplicationDbContext dbContext, [FromServices] IOmsEmailSender emailSender)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != default)
            {
                var st = isAccepted ? Status.Accepted : Status.Declined;
                user.Status = st;
                await dbContext.SaveChangesAsync();
                await emailSender.Send2ClientSignupResponseMessage(user.Email, st.ToString(),_logger);
                return NoContent();
            }
            return BadRequest(NotFound());
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("Disable/{userId}/{isDisabled}")]
        [SwaggerOperation("Enable/Disable specified user")]
        public async Task<IActionResult> Disable(long userId, bool disabled,[FromServices] ApplicationDbContext dbContext)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user != null)
            {
                if (user.Disabled != disabled)
                {
                    user.Disabled = disabled;
                    await dbContext.SaveChangesAsync();
                }
                return NoContent();
            }
            return BadRequest(NotFound());
        }
    }
}
