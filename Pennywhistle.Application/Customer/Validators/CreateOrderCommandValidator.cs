using FluentValidation;
using Pennywhistle.Application.Customer.Commands;

namespace Pennywhistle.Application.Customer.Validators
{
    /// <summary>
    /// Validatins for create command
    /// </summary>
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().MaximumLength(100);
            RuleFor(c => c.Size).NotNull().MaximumLength(50);
            RuleFor(c => c.TotalCost).NotNull();

        }
    }
}
