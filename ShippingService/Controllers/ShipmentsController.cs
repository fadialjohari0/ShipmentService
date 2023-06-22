using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Shipment;
using ShipmentService.API.UOW;
using ShipmentService.API.Validators;

namespace ShipmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PackageDtoValidator _packageValidator;
        private readonly ShipmentDtoValidator _shipmentValidator;

        public ShipmentsController(IUnitOfWork unitOfWork, IMapper mapper,
                                   PackageDtoValidator packageValidator, ShipmentDtoValidator shipmentValidator)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._packageValidator = packageValidator;
            this._shipmentValidator = shipmentValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetShipmentDto>>> GetAllShipments()
        {
            try
            {
                List<Shipment> shipments = await _unitOfWork.Shipments.GetDetailsAllAsync();
                List<GetShipmentsWithUserDto> record = _mapper.Map<List<GetShipmentsWithUserDto>>(shipments);
                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching shipments.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            try
            {
                Shipment shipment = await _unitOfWork.Shipments.GetDetails(id);

                if (shipment is null)
                {
                    var message = string.Format("Shipment with id = {0} not found", id);
                    return NotFound(message);
                }

                GetShipmentDto record = _mapper.Map<GetShipmentDto>(shipment);

                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching the shipment.");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostShipment(CreateShipmentDto createShipmentDto)
        {
            var PackageValidationResult = await _packageValidator.ValidateAsync(createShipmentDto.Package);

            if (!PackageValidationResult.IsValid)
            {
                var errors = PackageValidationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            var ShipmentValidationResult = await _shipmentValidator.ValidateAsync(createShipmentDto);

            if (!ShipmentValidationResult.IsValid)
            {
                var errors = ShipmentValidationResult.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            try
            {
                string userId = User.Claims.FirstOrDefault(x => x.Type == "id").Value;
                Shipment shipment = _mapper.Map<Shipment>(createShipmentDto);
                shipment.UserId = userId;

                await _unitOfWork.Shipments.AddAsync(shipment);
                await _unitOfWork.CompleteAsync();

                var response = new
                {
                    Message = "Shipment created successfully",
                    ShipmentId = shipment.Id
                };

                return CreatedAtAction("GetShipment", new { id = shipment.Id }, response);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the shipment.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteShipment(int id)
        {
            try
            {
                Shipment shipment = await _unitOfWork.Shipments.GetAsync(id);

                if (shipment is null)
                {
                    return NotFound($"Shipment {id} isn't found!");
                }

                await _unitOfWork.Shipments.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return Ok($"Shipment {shipment.Id} has been deleted!");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the shipment.");
            }
        }
    }
}
