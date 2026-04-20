using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ValidationFailureResponse
    {
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }

    public static class ValidationFailureMapper
    {
        public static ValidationFailureResponse ToResponse(this IEnumerable<ValidationFailure> failures)
        {
            return new ValidationFailureResponse
            {
                Errors = failures.Select(x => x.ErrorMessage)
            };
        }
    }
}
