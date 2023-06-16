using FluentValidation;
using ShipmentService.API.Models.Shipment;

namespace ShipmentService.API.Validators
{
    public class ShipmentDtoValidator : AbstractValidator<BaseShipmentDto>
    {
        public ShipmentDtoValidator()
        {
            RuleFor(s => s.ShippingCompany)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Shipment Company is required!")
                .Must(HaveValidCompany).WithMessage("Shipment Company should be either Fedex, or UPS.");

            RuleFor(s => s.ShippingServiceType)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Shipping Service is required!")
                .Must((dto, service) => HaveValidShippingService(dto.ShippingCompany, service))
                .WithMessage((dto, service) =>
                $"Wrong Shipping Service input for company {dto.ShippingCompany}! Valid options are: " +
                (dto.ShippingCompany.ToLower() == "fedex" ? "FedexGROUND, FedexAIR" : "UPSExpress, UPS2DAY"));
        }

        private bool HaveValidCompany(string company)
        {
            return company.ToLower() == "fedex" || company.ToLower() == "ups" ? true : false;
        }

        private bool HaveValidShippingService(string company, string service)
        {
            if (company.ToLower() == "fedex")
            {
                return service.ToLower() == "fedexground" || service.ToLower() == "fedexair" ? true : false;
            }
            else if (company.ToLower() == "ups")
            {
                return service.ToLower() == "upsexpress" || service.ToLower() == "ups2day" ? true : false;
            }
            return false;
        }
    }
}