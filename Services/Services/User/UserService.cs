using Common.Exceptions;
using Entities;
using Microsoft.AspNetCore.Identity;
using Services.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;

        public UserService(IJwtService jwtService, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Deleted = true;
            user.UserName += "- DELETED";
            user.NormalizedUserName += "- DELETED";
            await _userManager.UpdateAsync(user);
        }

        public async Task<AccessToken> TokenAsync(string username, string password, CancellationToken cancellationToken)
        {
            var existsUser = await _userManager.FindByNameAsync(username);
            if (existsUser == null)
            {
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(existsUser, password);
            if (!isPasswordValid)
            {
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
            }

            

            var jwt = await _jwtService.GenerateAsync(existsUser);
            return jwt;

        }
    }
}
