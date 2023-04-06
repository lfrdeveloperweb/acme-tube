using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;
using FluentValidation;
using FluentValidation.Results;

namespace AcmeTube.Application.Core.Commands
{
    public interface ICommandValidator<in TCommand>
    {
        Task<CommandValidationResult> ValidateCommandAsync(TCommand command);
    }

    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>, ICommandValidator<TCommand>
    {
        static CommandValidator()
        {
			ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        }

        /// <summary>
        /// Validate operation data contract.
        /// </summary>
        public virtual async Task<CommandValidationResult> ValidateCommandAsync(TCommand request)
        {
            var validationResult = await ValidateAsync(request);
            if (validationResult.IsValid) return CommandValidationResult.Succeeded;

			var errors = validationResult.Errors
				.Where(x => x.Severity == Severity.Error)
				.Select(CreateReport)
				.DefaultIfEmpty()
				.ToList();

			return CommandValidationResult.Failed(errors);
        }

        /// <summary>
        /// Create instance of <see cref="Report"/> from <see cref="ValidationFailure"/>.
        /// </summary>
        private static Report CreateReport(ValidationFailure failure)
        {
            // Set field with the property name.
            // Remove prefix "Data." from properties that begin with it.
            var field = Regex.Replace(failure.PropertyName ?? "", "^(Data\\.)(.*)$", "$2");

            // Replace in the properties than begin with "Data." by empty space.
            return new Report(default, field, failure.ErrorMessage);
        }
    }
}
