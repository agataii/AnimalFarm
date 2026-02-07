using AnimalFarm.Application.DTOs.Animal;
using FluentValidation;

namespace AnimalFarm.Application.Validators;

public class UpdateAnimalValidator : AbstractValidator<UpdateAnimalDto>
{
    public UpdateAnimalValidator()
    {
        RuleFor(x => x.InventoryNumber)
            .NotEmpty().WithMessage("Inventory number is required.")
            .MaximumLength(50).WithMessage("Inventory number must not exceed 50 characters.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(g => g == "Male" || g == "Female")
            .WithMessage("Gender must be 'Male' or 'Female'.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.ArrivalDate)
            .NotEmpty().WithMessage("Arrival date is required.");

        RuleFor(x => x.ArrivalAgeMonths)
            .GreaterThanOrEqualTo(0).WithMessage("Arrival age must be non-negative.");

        RuleFor(x => x.BreedId)
            .GreaterThan(0).WithMessage("BreedId must be a positive number.");

        RuleFor(x => x.ParentAnimalId)
            .GreaterThan(0).When(x => x.ParentAnimalId.HasValue)
            .WithMessage("ParentAnimalId must be a positive number.");
    }
}
