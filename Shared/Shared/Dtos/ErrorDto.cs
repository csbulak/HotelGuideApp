using Shared.Enums;
using Shared.Helpers;

namespace Shared.Dtos;

public class ErrorDto
{
    public ErrorDto(ErrorCodes errorCode)
    {
        ErrorCode = errorCode;
        ErrorMessage = Helper.GetEnumDescription(errorCode);
    }

    public ErrorCodes ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}