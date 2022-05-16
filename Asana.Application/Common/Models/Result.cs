using System;
using System.Collections.Generic;
using System.Linq;

namespace Asana.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors , object value = default)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Value = value;
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public object Value { get; set; }

        public static Result Success(object value = default)
        {
            return new Result(true, Array.Empty<string>() , value);
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(false, errors);
        }
        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }

    }
}
