using Application.Exceptions;
using Application.Models.Wrapper;
using System.Net;
using System.Text.Json;

namespace WebApi
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {

                var response = httpContext.Response;
                response.ContentType = "application/json";
                var responseWrapper = await ResponseWrapper<string>.FailAsync(ex.Message);
                switch (ex)
                {
                    case ConflictException conflictException:
                        response.StatusCode = (int) conflictException.StatusCode;
                        break;
                    case NotFoundException notFound:
                        response.StatusCode= (int) notFound.StatusCode;
                        break;
                        case ForbiddenException forbiddenException:
                        response.StatusCode=(int) forbiddenException.StatusCode;
                        break;
                        case IdentityException identityException:
                        response.StatusCode =(int) identityException.StatusCode;
                        break;
                        case UnauthorizedException unauthorizedException:
                        response.StatusCode = (int)unauthorizedException.StatusCode;
                        break;
                        default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

              
                var jsonResponse = JsonSerializer.Serialize(responseWrapper);
               await response.WriteAsync(jsonResponse);
            }
        }
    }
}
