using AnimalFarm.Application.DTOs.AnimalType;
using FluentValidation;

namespace AnimalFarm.Application.Validators;

public class UpdateAnimalTypeValidator : AbstractValidator<UpdateAnimalTypeDto>
{
    public UpdateAnimalTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
