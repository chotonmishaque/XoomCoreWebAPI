namespace XoomCore.Application.Common.Response;
public class CommonResponse<TResponse>
{
    [JsonPropertyName("responseStatus")]
    public bool ResponseStatus { get; set; }
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }
    [JsonPropertyName("messageType")]
    public required string MessageType { get; set; }
    [JsonPropertyName("message")]
    public required string Message { get; set; }
    [JsonPropertyName("data")]
    public required TResponse Data { get; set; }
    [JsonPropertyName("validationErrors")]
    public required List<ValidationError> ValidationErrors { get; set; }

    public static CommonResponse<TResponse> CreateSuccess(string message = "Success")
    {
        return CreateResponse(statusCode: (int)ResponseType.Success, responseType: ResponseType.Success, data: default, message: message);
    }
    public static CommonResponse<TResponse> CreateSuccess(TResponse data = default, string message = "Success")
    {
        return CreateResponse(statusCode: (int)ResponseType.Success, responseType: ResponseType.Success, data: data, message: message);
    }
    public static CommonResponse<TResponse> CreateWarning(int statusCode, string message = "Warning")
    {
        return CreateResponse(statusCode: statusCode, responseType: ResponseType.Warning, data: default, message: message);
    }
    public static CommonResponse<TResponse> CreateWarning(string message = "Warning")
    {
        return CreateResponse(statusCode: (int)ResponseType.Warning, responseType: ResponseType.Warning, data: default, message: message);
    }
    public static CommonResponse<TResponse> CreateError(int statusCode, string message = "Error")
    {
        return CreateResponse(statusCode: statusCode, responseType: ResponseType.Error, data: default, message: message);
    }
    public static CommonResponse<TResponse> CreateError(string message = "Error")
    {
        return CreateResponse(statusCode: (int)ResponseType.Error, responseType: ResponseType.Error, data: default, message: message);
    }
    public static CommonResponse<TResponse> CreateValidationErrorList(FluentValidation.Results.ValidationResult validationResult)
    {
        var responseMessage = "Some required information is missing or contains invalid data! Please check your input.";
        return CreateResponse(statusCode: (int)ResponseType.ValidationError, responseType: ResponseType.ValidationError, data: default, message: responseMessage, CreateValidationErrors(validationResult));
    }

    public static CommonResponse<TResponse> CreateValidationErrorSingle(string errorCode, string errorMessage)
    {
        var responseMessage = "Some required information is missing or contains invalid data! Please check your input.";
        return CreateResponse(statusCode: (int)ResponseType.ValidationError, responseType: ResponseType.ValidationError, data: default, message: responseMessage, new List<ValidationError> { CreateValidationError(errorCode, errorMessage) });
    }

    private static List<ValidationError> CreateValidationErrors(FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult?.Errors?.Select(error => CreateValidationError(error.PropertyName, error.ErrorMessage)).ToList();
    }

    private static ValidationError CreateValidationError(string errorCode, string errorMessage)
    {
        return new ValidationError
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
    }

    public static CommonResponse<TResponse> CreateResponse(int responseCode)
    {
        string message;
        var responseType = GetResponseTypeFromDatabase(responseCode, out message);

        return CreateResponse(1, responseType, default, message);
    }

    private static CommonResponse<TResponse> CreateResponse(int statusCode, ResponseType responseType, TResponse data = default, string message = null, List<ValidationError> validationErrors = default)
    {
        return new CommonResponse<TResponse>
        {
            StatusCode = statusCode,
            MessageType = responseType.ToString(),
            ResponseStatus = responseType == ResponseType.Success,
            Message = message ?? "Something went wrong!",
            Data = data,
            ValidationErrors = validationErrors ?? new List<ValidationError>() // Set to an empty list if validationErrors is not provided
        };
    }

    private static ResponseType GetResponseTypeFromDatabase(int responseCode, out string message)
    {
        // Implement your logic to retrieve response type and message from the database based on responseCode
        // Query the database based on responseCode
        // Retrieve the response type and message
        // The 'message' parameter should contain the message retrieved from the database

        // For demonstration purposes, we'll use a simple switch statement
        switch (responseCode)
        {
            case 1:
                message = "Success message from the database";
                return ResponseType.Success;
            case 2:
                message = "Warning message from the database";
                return ResponseType.Warning;
            default:
                message = "Unrecognized response code";
                return ResponseType.Error;
        }
    }

    public class ValidationError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}

public enum ResponseType
{
    Success = 200,
    Warning = 201,
    Error = 400,
    ValidationError = 403
}