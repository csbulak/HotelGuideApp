using System.Net;
using Shared.Enums;

namespace Shared.Dtos;

public class Response<T>
{
    public T? Data { get; set; }

    public HttpStatusCode StatusCode { get; private init; }

    public Boolean IsSuccessful { get; private init; }

    public List<ErrorDto>? Errors { get; set; }

    public static Response<T> Success(T data)
    {
        return new Response<T> { Data = data, StatusCode = HttpStatusCode.OK, IsSuccessful = true };
    }

    public static Response<T> Success(T data, HttpStatusCode statusCode)
    {
        return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Response<T> Fail(List<ErrorDto>? errors, HttpStatusCode statusCode)
    {
        return new Response<T> { Errors = errors, StatusCode = statusCode, IsSuccessful = false };
    }

    public static Response<T> Fail(ErrorCodes errorCode, HttpStatusCode statusCode)
    {
        return new Response<T> { Errors = new List<ErrorDto> { new ErrorDto(errorCode) }, StatusCode = statusCode, IsSuccessful = false };
    }
}