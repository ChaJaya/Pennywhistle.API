using FluentValidation;
using Pennywhistle.Application.Products.Commands;

namespace Pennywhistle.Application.Products.Validators
{
    /// <summary>
    /// validations for create product
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().MaximumLength(100);
            RuleFor(c => c.Size).NotNull().MaximumLength(50);
          
        }
    }
}
