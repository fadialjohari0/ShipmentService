using Microsoft.AspNetCore.Identity;
using ShipmentService.API.Models.Users;

namespace ShipmentService.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<bool> Login(LoginDto loginDto);
    }
}