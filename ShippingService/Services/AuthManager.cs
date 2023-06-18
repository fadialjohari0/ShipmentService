using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Users;

namespace ShipmentService.API.AuthManager
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
        {
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return default;
            }

            var isValidCredentials = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isValidCredentials)
            {
                return default;
            }
            return true;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }
    }
}