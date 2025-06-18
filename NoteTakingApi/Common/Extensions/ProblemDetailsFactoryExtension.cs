using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NoteTakingApi.Common.Exceptions;
using ApplicationException = NoteTakingApi.Common.Exceptions.ApplicationException;

namespace NoteTakingApi.Common.Extensions;


public static class ProblemDetailsFactoryExtension
{
    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory,
        HttpContext httpContext,
        ApplicationException applicationException)
    {
        return factory.CreateProblemDetails(httpContext, applicationException.ErrorCode switch
        {
            ErrorCodes.NotFound => StatusCodes.Status404NotFound,
            ErrorCodes.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        }, applicationException.Message);
        
        
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        FluentValidation.ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }
        return factory.CreateValidationProblemDetails(httpContext, modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Invalid Request", detail: validationException.Message);
    }
}