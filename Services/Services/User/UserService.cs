using Common.Exceptions;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IRepository<User> _repository;
        private readonly UserManager<User> _userManager;

        public UserService(IJwtService jwtService, IRepository<User> repository, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _repository = repository;
            _userManager = userManager;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByIdAsync(id);
            user.Deleted = true;
            user.UserName += "- DELETED";
            user.NormalizedUserName += "- DELETED";
            await _userManager.UpdateAsync(user);
            await _repository.UpdateAsync(user, cancellationToken);
        }

        public async Task<AccessToken> TokenAsync(string username, string password, CancellationToken cancellationToken)
        {
            User existsUser = await _userManager.FindByNameAsync(username);
            if (existsUser == null)
            {
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(existsUser, password);
            if (!isPasswordValid)
            {
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
            }



            AccessToken jwt = await _jwtService.GenerateAsync(existsUser);
            return jwt;

        }
    }
}
