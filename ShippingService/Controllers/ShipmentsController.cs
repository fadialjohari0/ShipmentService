using System.Net;
using AutoMapper;
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

        public ShipmentsController(IUnitOfWork unitOfWork, IMapper mapper, PackageDtoValidator packageValidator)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._packageValidator = packageValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetShipmentDto>>> GetAllShipments()
        {
            try
            {
                var shipments = await _unitOfWork.Shipments.GetDetailsAllAsync();
                var record = _mapper.Map<List<GetShipmentDto>>(shipments);
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
                var shipment = await _unitOfWork.Shipments.GetDetails(id);

                if (shipment is null)
                {
                    var message = string.Format("Shipment with id = {0} not found", id);
                    return NotFound(message);
                }

                var record = _mapper.Map<GetShipmentDto>(shipment);

                return Ok(record);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching the shipment.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostShipment(CreateShipmentDto createShipmentDto)
        {
            try
            {
                Shipment shipment = _mapper.Map<Shipment>(createShipmentDto);

                var validationResult = await _packageValidator.ValidateAsync(createShipmentDto.Package);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage);
                    return BadRequest(errors);
                }

                string message = ShipmentInputValidation(createShipmentDto);

                if (message != null)
                {
                    return BadRequest(message);
                }

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

        private string ShipmentInputValidation(CreateShipmentDto createShipmentDto)
        {
            string fedexShipment = "fedex";
            bool isFedexCarrierService = createShipmentDto.CarrierServiceId.ToLower() == "fedexair" ||
                                         createShipmentDto.CarrierServiceId.ToLower() == "fedexground";

            string upsShipment = "ups";
            bool isUPSCarrierService = createShipmentDto.CarrierServiceId.ToLower() == "upsexpress" ||
                                       createShipmentDto.CarrierServiceId.ToLower() == "ups2day";

            if (createShipmentDto.ShipmentId.ToLower() == fedexShipment && !isFedexCarrierService)
            {
                return "Wrong carrier service input! FedEx has the following Carrier Services: 'fedexAIR' or 'fedexGROUND'";
            }

            if (createShipmentDto.ShipmentId.ToLower() == upsShipment && !isUPSCarrierService)
            {
                return "Wrong carrier service input! UPS has the following Carrier Services: 'UPSExpress' or 'UPS2DAY'";
            }

            return null;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShipment(int id)
        {
            try
            {
                var shipment = await _unitOfWork.Shipments.GetAsync(id);

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
