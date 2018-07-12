using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using CDEXAPIcore.Pages.Account;
using CDEXAPIcore.Services;
using CDEXAPIcore.Data;

namespace CDEXAPIcore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IConfiguration Configuration;

        public UsersController(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            Configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Post([FromBody]LoginModel.InputModel request)
        {
            if (ModelState.IsValid)
            {
                //get user from request 

                var user = await _userManager.FindByEmailAsync(request.Email);
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: Configuration["JWTissuer"],
                    audience: Configuration["JWTaudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    firstname = "",
                    lastname = "",
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest();
        }


    }
}