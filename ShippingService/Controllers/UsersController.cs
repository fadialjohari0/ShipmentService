using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Users;
using ShipmentService.API.UOW;

namespace ShipmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IAuthManager authManager)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            AuthResponseDto authResponse = await _authManager.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized("Wrong Credentials");
            }

            return Ok(authResponse);
        }

        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            AuthResponseDto authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized("Wrong Credentials");
            }

            return Ok(authResponse);
        }

        [HttpGet]
        [Route("GetUsers")]
        // [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetAllUsers()
        {
            try
            {
                List<ApiUser> users = await _unitOfWork.Users.GetDetailsAllAsync();
                var record = _mapper.Map<List<GetUsersDto>>(users);

                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching shipments.");
            }
        }

        [HttpGet]
        [Route("GetUsersShipments")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetAllUsersWithShipments()
        {
            try
            {
                List<ApiUser> users = await _unitOfWork.Users.GetDetailsAllAsync();
                var record = _mapper.Map<List<GetUsersShipmentsDto>>(users);

                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching shipments.");
            }
        }
    }
}