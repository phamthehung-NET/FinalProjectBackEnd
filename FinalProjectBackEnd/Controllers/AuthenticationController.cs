using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Services.Implementations;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Model.DTO;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly IHubContext<SignalR> hubContext;

        public AuthenticationController(UserManager<CustomUser> _userManager, 
            IConfiguration _configuration, 
            IHttpContextAccessor _httpContextAccessor,
            //IHubContext<SignalR> _hubContext,
            IUserService _userService)
        {
            userManager = _userManager;
            //hubContext = _hubContext;
            configuration = _configuration;
            httpContextAccessor = _httpContextAccessor;
            userService = _userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserID", user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expration = token.ValidTo
                });
            }
            return Unauthorized("UserName and Password is not correct");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var userId = httpContextAccessor.HttpContext.User.Claims.ElementAt(1).Value;
            var currentUser = userService.GetCurrentUser(userId).FirstOrDefault();
            return Ok(currentUser);
        }

        [HttpGet]
        public IActionResult SeedData()
        {
            userService.SeedData();
            return Ok();
        }
    }
}
