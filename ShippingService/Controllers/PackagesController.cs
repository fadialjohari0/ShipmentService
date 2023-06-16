using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Package;
using ShipmentService.API.UOW;
using ShipmentService.API.Validators;

namespace ShipmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PackageDtoValidator _packageValidator;

        public PackagesController(IUnitOfWork unitOfWork, IMapper mapper, PackageDtoValidator packageValidator)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._packageValidator = packageValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllPackagesDto>>> GetAllPackages()
        {
            try
            {
                var packages = await _unitOfWork.Packages.GetAllAsync();
                var record = _mapper.Map<List<GetAllPackagesDto>>(packages);
                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching packages.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackage(int id)
        {
            var package = await _unitOfWork.Packages.GetAsync(id);

            if (package is null)
            {
                return NotFound($"Package with id {id} not found!");
            }

            var record = _mapper.Map<PackageDto>(package);
            return Ok(record);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Package>> UpdatePackage(int id, PackageDto packageDto)
        {
            var package = await _unitOfWork.Packages.GetAsync(id);

            if (package is null)
            {
                return NotFound($"Package with id {id} not found!");
            }

            var validationResult = await _packageValidator.ValidateAsync(packageDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _mapper.Map(packageDto, package);
            _unitOfWork.Packages.Update(package);
            await _unitOfWork.CompleteAsync();

            return Ok(package);
        }
    }
}