using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Asana.Application.Utilities.Common
{
    public static class JsonResponseStatus
    {
        public static JsonResult Success()
        {
            return new JsonResult((new { Status = "Success" }))
            { StatusCode = StatusCodes.Status200OK};
        }

        public static JsonResult Success(object returnData)
        {
            return new JsonResult(new { Status = "Success" , Data = returnData })
            { StatusCode=StatusCodes.Status200OK};
        }

        public static JsonResult NotFound()
        {
            return new JsonResult(new { Status = "NotFound" })
            {StatusCode=StatusCodes.Status404NotFound };
        }

        public static JsonResult NotFound(object returnData)
        {
            return new JsonResult(new { Status = "NotFound", Data = returnData })
            { StatusCode = StatusCodes.Status404NotFound };
        }

        public static JsonResult Error()
        {
            return new JsonResult(new { Status = "Error" })
            { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public static JsonResult Error(object returnData)
        {
            return new JsonResult(new { Status = "Error", Data = returnData })
            { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public static JsonResult BadRequest(IEnumerable<string> errors)
        {
            return new JsonResult(new { Status = "BadRequest", Errors = errors })
            { StatusCode = StatusCodes.Status400BadRequest, };
        }

        public static JsonResult SuccessCreated()
        {
            return new JsonResult(new { Status = "SuccessCreated" })
            {StatusCode =StatusCodes.Status201Created };
        }

        public static JsonResult Unauthorized()
        {
            return new JsonResult( new { Status = "Unauthorized" })
            { StatusCode = StatusCodes.Status401Unauthorized };
        }

    }
}
