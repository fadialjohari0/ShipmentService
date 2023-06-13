using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShipmentService.API.Contracts;
using ShipmentService.API.Data;
using ShipmentService.API.Models.Shipment;
using ShipmentService.API.Validators;

namespace ShipmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShipmentsRepository _shipmentsRepository;
        private readonly PackageDtoValidator _packageValidator;

        public ShipmentsController(IMapper mapper, IShipmentsRepository shipmentsRepository, PackageDtoValidator packageValidator)
        {
            this._mapper = mapper;
            this._shipmentsRepository = shipmentsRepository;
            this._packageValidator = packageValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetShipmentDto>>> GetShipments()
        {
            try
            {
                var shipments = await _shipmentsRepository.GetDetailsAllAsync();
                var record = _mapper.Map<List<GetShipmentDto>>(shipments);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching shipments.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            try
            {
                var shipment = await _shipmentsRepository.GetDetails(id);

                if (shipment is null)
                {
                    var message = string.Format("Shipment with id = {0} not found", id);
                    return NotFound(message);
                }

                return Ok(shipment);
            }
            catch (Exception ex)
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

                await _shipmentsRepository.AddAsync(shipment);

                var response = new
                {
                    Message = "Shipment created successfully",
                    ShipmentId = shipment.Id
                };

                return CreatedAtAction("GetShipment", new { id = shipment.Id }, response);
            }
            catch (Exception ex)
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
                var shipment = await _shipmentsRepository.GetAsync(id);

                if (shipment is null)
                {
                    return NotFound($"Shipment {id} isn't found!");
                }

                await _shipmentsRepository.DeleteAsync(id);
                return Ok($"Shipment {shipment.Id} has been deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the shipment.");
            }
        }
    }
}
