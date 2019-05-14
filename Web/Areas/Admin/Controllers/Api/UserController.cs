using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Dto;
using Services.Enum.ServicesResult.Student;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Filters;

namespace Web.Areas.Admin.Controllers.Api
{
    [Route("api/[area]/[controller]/[action]")]
    [Area("Admin")]
    [ApiController]
   // [Authorize(Roles ="Admin")]
    [ApiResultFilter]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IStudentService _studentService;
        private readonly RoleManager<Role> _roleManager;

        public UserController(UserManager<User> userManager, IStudentService studentService, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _studentService = studentService;
            _roleManager = roleManager;
        }
 
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentDto studentDto, [FromHeader]CancellationToken cancellationToken)
        {
            var existsStudentCode = await _studentService.GetByStudentNumberAsync(cancellationToken, studentDto.UserName);
            if (existsStudentCode != null)
                return BadRequest("شماره دانشجویی تکراری است");

            var existsStudentId = await _userManager.FindByIdAsync(studentDto.Id);
            if (existsStudentId != null)
                return BadRequest("این دانشجو از قبل موجود است");

            var existsStudentUserName = await _userManager.FindByNameAsync(studentDto.UserName);
            if (existsStudentUserName != null)
                return BadRequest("این شماره دانشجویی قبلا ثبت شده است");

            var user = new User
            {
                UserName = studentDto.UserName,
                FullName = studentDto.FullName
            };
            var result = await _userManager.CreateAsync(user, "111111");
            if (result != IdentityResult.Success)
                return BadRequest("مشکلی در ثبت نام به وجود آمده است");

            var student = studentDto.ToEntity();
            studentDto.UserId = user.Id;
            studentDto.EntryYear = int.Parse(studentDto.UserName.Substring(0, 2));
            var studentResult = await _studentService.AddAsync(cancellationToken, studentDto);
            if (studentResult == AddAsyncStatus.Exists)
                return BadRequest("این دانشجو از قبل موجود بوده است");

            if (studentResult == AddAsyncStatus.Bad)
                return BadRequest("مشکلی در ثبت نام به وجود آمده است");

            var roleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (roleResult != IdentityResult.Success)
                return BadRequest("مشکلی در ثبت نام به وجود آمده است");

            return Ok($"ثبت نام {user.FullName} با موفیت انجام شد");
        }

    }
}