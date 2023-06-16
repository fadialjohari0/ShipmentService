using FluentValidation;
using ShipmentService.API.Models.Package;

namespace ShipmentService.API.Validators
{
    public class PackageDtoValidator : AbstractValidator<BasePackageDto>
    {
        public PackageDtoValidator()
        {
            RuleFor(p => p.Width)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Width is required!")
                .Must(BeNumeric).WithMessage("Width must be an integer greater than 0.");

            RuleFor(p => p.Height)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Height is required!")
                .Must(BeNumeric).WithMessage("Height must be an integer greater than 0.");

            RuleFor(p => p.Length)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Length is required!")
                .Must(BeNumeric).WithMessage("Length must be an integer greater than 0.");

            RuleFor(p => p.Weight)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Width is required!")
                .Must(BeNumeric).WithMessage("Weight must be an integer greater than 0.");
        }

        private bool BeNumeric(double value)
        {
            return value > 0;
        }
    }
}
