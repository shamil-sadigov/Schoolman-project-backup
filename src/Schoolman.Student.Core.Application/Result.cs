using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schoolman.Student.Core.Application
{
    public class Result
    {
        public bool Succeeded { get; private set; }
        public string[] Errors { get; private set; }

        private Result(bool succeeded, string[] errors)
            => (Succeeded, Errors) = (succeeded, errors);

        public static Result Success()=>
            new Result(true, Array.Empty<string>());

        public static Result Failure(params string[] errors) =>
            new Result(false, errors);

    }
}
