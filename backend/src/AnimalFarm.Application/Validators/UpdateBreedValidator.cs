using AnimalFarm.Application.DTOs.Breed;
using FluentValidation;

namespace AnimalFarm.Application.Validators;

public class UpdateBreedValidator : AbstractValidator<UpdateBreedDto>
{
    public UpdateBreedValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.AnimalTypeId)
            .GreaterThan(0).WithMessage("AnimalTypeId must be a positive number.");
    }
}
