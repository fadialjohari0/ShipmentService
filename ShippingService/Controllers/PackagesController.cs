using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Package;
using ShipmentService.API.Validators;

namespace ShipmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPackagesRepository _packagesRepository;
        private readonly PackageDtoValidator _packageValidator;

        public PackagesController(IMapper mapper, IPackagesRepository packagesRepository, PackageDtoValidator packageValidator)
        {
            this._mapper = mapper;
            this._packagesRepository = packagesRepository;
            this._packageValidator = packageValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllPackagesDto>>> GetAllPackages()
        {
            try
            {
                var packages = await _packagesRepository.GetAllAsync();
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
            var package = await _packagesRepository.GetAsync(id);

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
            var package = await _packagesRepository.GetAsync(id);

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
            await _packagesRepository.UpdateAsync(package);

            return Ok(package);
        }
    }
}