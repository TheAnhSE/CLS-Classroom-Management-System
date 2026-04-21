namespace CLS.Common;

public class BaseResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public BaseResponse(int code, string message, T? data = default)
    {
        Code = code;
        Message = message;
        Data = data;
    }
    
    public static BaseResponse<T> Success(T data, string message = "Success", int code = 200)
    {
        return new BaseResponse<T>(code, message, data);
    }

    public static BaseResponse<T> Failure(string message, int code)
    {
        return new BaseResponse<T>(code, message, default);
    }
}
