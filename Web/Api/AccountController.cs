using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace Web.Api
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    //[ApiResultFilter]
    //[CustomExceptionFilter]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(/*[FromHeader]CancellationToken cancellationToken, string username, string password*/)
        {
            //var accessToken = await _userService.TokenAsync(username, password, cancellationToken);
            ////var user = await _userManager.FindByNameAsync(username);
            ////if (user == null)
            ////    return BadRequest("کاربر مورد نظر یافت نشد");
            ////return Ok();
            //return Ok(accessToken);
            return BadRequest();
        }

    }
}