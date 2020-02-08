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



        /// <summary>
        /// Return wether result suceeded
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(Result result)
        {
            return result.Succeeded;
        }

        /// <summary>
        /// Returns only errors
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator string[](Result result)
        {
            return result.Errors;
        }


        public static implicit operator Result(bool succeeede)
        {
            return Result.Success();
        }
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
            return new Result<T>(null, false, Array.Empty<string>());
        }





        public static implicit operator T (Result<T> result)
        {
            return result.Response;
        }


        /// <summary>
        /// Return wether result suceeded
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(Result<T> result)
        {
            return result.Succeeded;
        }

        /// <summary>
        /// Returns only errors
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator string[](Result<T> result)
        {
            return result.Errors;
        }

        public static implicit operator Result<T>(T response)
        {
            return Result<T>.Success(response);
        }

        public static implicit operator Result<T>(string error)
        {
            return Result<T>.Failure(error);
        }

    }



}
