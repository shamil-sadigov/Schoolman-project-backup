using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schoolman.Student.Core.Application.Models
{
    /// <summary>
    /// Simple Result object that is used in common cases
    /// </summary>
    public class Result
    {
        public bool Succeeded { get; protected set; }
        public string[] Errors { get; protected set; }

        protected Result(bool succeeded, string[] errors)
            => (Succeeded, Errors) = (succeeded, errors);

        public static Result Success()=>
            new Result(true, Array.Empty<string>());

        public static Result Failure(params string[] errors) =>
            new Result(false, errors);

    }


    public class Result<T> : Result where T:class
    {
        public T Response;

        private Result(T response, bool succeeded, string[] errors):base(succeeded, errors)
        {
            Response = response;
        }


        public static Result<T> Success(T reponse)
        {
            return new Result<T>(reponse, true, Array.Empty<string>());
        }

        public new static Result<T> Failure(params string[] errors)
        {
            return new Result<T>(null, true, Array.Empty<string>());
        }


    }



}
