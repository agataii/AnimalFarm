using AnimalFarm.Application.DTOs.Weighting;
using FluentValidation;

namespace AnimalFarm.Application.Validators;

public class CreateWeightingValidator : AbstractValidator<CreateWeightingDto>
{
    public CreateWeightingValidator()
    {
        RuleFor(x => x.AnimalId)
            .GreaterThan(0).WithMessage("AnimalId must be a positive number.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.WeightKg)
            .GreaterThan(0).WithMessage("Weight must be greater than zero.");
    }
}
