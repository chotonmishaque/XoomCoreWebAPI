namespace XoomCore.Application.Common.Response;


public class CommonDataTableResponse<TResponse>
{
    public bool IsValid { get; set; }
    public int StatusCode { get; set; }
    public string MessageType { get; set; }
    public string Message { get; set; }
    public long TotalRowCount { get; set; } = 0;
    public long NoOfRecordsFetched { get; set; } = 0;
    public TResponse Data { get; set; }

    public static CommonDataTableResponse<TResponse> CreateSuccess(long totalRowCount = 0, TResponse data = default, string message = "Success")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = 200,
            IsValid = true,
            MessageType = "Success",
            Message = message,
            TotalRowCount = totalRowCount,
            Data = data
        };
    }

    public static CommonDataTableResponse<TResponse> CreateSuccess(long totalRowCount = 0, long noOfRecordsFetched = 0, TResponse data = default, string message = "Success")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = 200,
            IsValid = true,
            MessageType = "Success",
            Message = message,
            TotalRowCount = totalRowCount,
            NoOfRecordsFetched = noOfRecordsFetched,
            Data = data
        };
    }

    public static CommonDataTableResponse<TResponse> CreateWarning(int responseCode = 200, string message = "No results were found!")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = responseCode,
            IsValid = true,
            MessageType = "Warning",
            Message = message,
            Data = default
        };
    }

    public static CommonDataTableResponse<TResponse> CreateError(int errorCode, string errorMessage = "Error")
    {
        return new CommonDataTableResponse<TResponse>
        {
            StatusCode = errorCode,
            IsValid = false,
            MessageType = "Error",
            Message = errorMessage,
            Data = default
        };
    }
}