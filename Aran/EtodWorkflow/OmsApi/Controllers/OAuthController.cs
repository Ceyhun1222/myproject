using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OmsApi.Data;
using OmsApi.Dto;
using OmsApi.Entity;
using OmsApi.Configuration;
using OmsApi.Interfaces;
using Swashbuckle.AspNetCore;
using OmsApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Encodings.Web;

namespace OmsApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class OAuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OAuthController> _logger;
        private readonly IConfiguration _configuration;

        public OAuthController(UserManager<ApplicationUser> userManager,
            ILogger<OAuthController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation("Gets access token and refresh token")]
        [SwaggerResponse((int)StatusCodes.Status401Unauthorized)]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResult>> Login(Login credential, [FromServices] ISignInManager signInManager, 
            [FromServices] ITokenService tokenService, [FromServices] ApplicationDbContext dbContext)
        {
            var user = await signInManager.SignIn(credential.UserName, credential.Password);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles?.Count > 0)
                {
                    if(credential.Role == Roles.Client)
                    {
                        if(roles[0] != nameof(Roles.Client))
                            return Unauthorized();
                    }
                    else{
                        if (roles[0] != nameof(Roles.Admin))
                            return Unauthorized();
                    }

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, credential.UserName),
                        new Claim(ClaimTypes.Role, roles[0])
                    };
                    var jwtToken = tokenService.GenerateAccessToken(claims);
                    var refreshToken = tokenService.GenerateRefreshToken();

                    user.RefreshToken = refreshToken;
                    await _userManager.UpdateAsync(user);
                    var slot = await dbContext.GetSelectedSlot();
                    return Ok(
                        new LoginResult()
                        {
                            Access = jwtToken,
                            Refresh = refreshToken,
                            Id = user.Id,
                            FullName = $"{user.Firstname} {user.Lastname}",
                            IsDefinedSlot = (slot != null)
                        });
                }
                else
                    return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation("Logs out")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogOut()
        {
            var user = await GetUser(_userManager);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Sign up")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status409Conflict,"Username exists")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent,"Successfully created")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest,"Invalid model")]
        public async Task<IActionResult> SignUp(UserRegistration registrationData,[FromServices]IMapper mapper,
            [FromServices]ApplicationDbContext dbContext, [FromServices] IOmsEmailSender emailSender)
        {
            var user = await _userManager.FindByNameAsync(registrationData.UserName);
            if (user != default)
                return Conflict();

            try
            {
                user = mapper.Map<ApplicationUser>(registrationData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Signup error");
                throw;
            }            
            var identityResult = await _userManager.CreateAsync(user,registrationData.Password);
            
            if (!identityResult.Succeeded)
            {
                GetModelStateFor(identityResult);
                return BadRequest(ModelState);
            }
            
            var res = await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
            if (!res.Succeeded)
            {
                GetModelStateFor(identityResult);
                return BadRequest(ModelState);
            }
            //await dbContext.UserNotifications.AddAsync(new UserNotification(user));
            await dbContext.SaveChangesAsync();
            await emailSender.Send2AdminSignupMessage(user.UserName, $"{user.Firstname} {user.Lastname}", _logger);
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Username checking")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CheckUsername(string username)
        {
            var res = await _userManager.FindByNameAsync(username);
            if (res != null)
                return Conflict();            
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Email checking")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        [SwaggerResponse((int)StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CheckEmail(string email)
        {
            var res = await _userManager.FindByEmailAsync(email);
            if (res != null)
                return Conflict();
            return NoContent();
        }

        [HttpPost]
        [SwaggerOperation("Refreshes access token and refresh token")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Tokens>> Refresh(Tokens oldCredentials,
            [FromServices] ITokenService tokenService, [FromServices]IConfiguration configuration)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(oldCredentials.Access, configuration);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != oldCredentials.Refresh) return BadRequest();

            var newJwtToken = tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new Tokens()
            {
                Access = newJwtToken,
                Refresh = newRefreshToken
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Forgot password")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ForgotPassword(ForgotPasword recoverData, 
            [FromServices] IOmsEmailSender emailSender)
        {
            var user = await _userManager.FindByEmailAsync(recoverData.Email);
            if (user == null)
                return BadRequest();
            var code =  await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebUtility.UrlEncode(code);
            var domain = _configuration[ConfigKeys.Domain4Client];
            if (await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
                domain = _configuration[ConfigKeys.Domain4Admin];            
            //_logger.LogInformation($"Request is :{Request.Scheme}://{Request.Host.Host.ToString()}:{Request.Host.Port}");
            //_logger.LogInformation($"Response is :{Response.HttpContext.Response}:{Response.HttpContext.Response}");
            //domain = Request.Scheme + "://" + Request.Host.ToString();
            var email = WebUtility.UrlEncode(user.Email);
            var callbackUrl = domain + $"/reset-password/{code}/{email}";
            await emailSender.Send2ClientForgotPasswordMessage(recoverData.Email, HtmlEncoder.Default.Encode(callbackUrl), _logger);
            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Reset password")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var email = WebUtility.UrlDecode(resetPassword.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(NotFound());
            var code = WebUtility.UrlDecode(resetPassword.Code);
            var res = await _userManager.ResetPasswordAsync(user, code, resetPassword.Password);
            if (!res.Succeeded)
            {
                _logger.LogInformation(res.Errors.ToString());
                GetModelStateFor(res);
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}