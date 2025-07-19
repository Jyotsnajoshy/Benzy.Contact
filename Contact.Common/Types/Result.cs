using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contact.Common.Types
{


    public class Result<TData>
    {
        public bool Success => ResultType == ResultType.Success; // Fix for CS0102: Retain the property definition.
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public TData? Data { get; set; }

        [JsonIgnore]
        public ResultType ResultType { get; set; }

        /// <summary>
        /// Initializes a <see cref="Result{TData}"/> object with Success result type.
        /// </summary>
        public Result()
        {
            ResultType = ResultType.Success;
        }

        /// <summary>
        /// Initializes a <see cref="Result{TData}"/> object with the specified result type.
        /// </summary>
        /// <param name="resultType">Type of the result.</param>
        public Result(ResultType resultType)
        {
            ResultType = resultType;
        }

        /// <summary>
        /// Initializes a <see cref="Result{TData}"/> object with Success result type and the data.
        /// </summary>
        /// <param name="data">The result returned from the repository.</param>
        public Result(TData data)
        {
            Data = data;
            ResultType = ResultType.Success;
        }

        /// <summary>
        /// Adds an error to the result and sets the result type.
        /// </summary>
        public Result<TData> AddError(string error, ResultType type)
        {
            Errors.Add(error);
            ResultType = type;
            return this;
        }

        public IActionResult ToActionResult()
        {
            switch (ResultType)
            {
                case ResultType.Success:
                case ResultType.CompletedWithErrors:
                    return new OkObjectResult(this);
                case ResultType.NotFound:
                    return new NotFoundObjectResult(this);
                case ResultType.InvalidData:
                    return new BadRequestObjectResult(this);
                case ResultType.PermissionDenied:
                    return new ForbidResult();
                default:
                    return new BadRequestObjectResult(this);
            }
        }

        // Fix for CS0234 and CS0102: Remove the conflicting static method definition.
    }
}

