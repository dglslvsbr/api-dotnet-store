namespace StoreAPI.Useful;

public class ServiceResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Exception? Exception { get; set; }

    public static ServiceResult Ok() => new() { Success = true };
    public static ServiceResult Fail(string message) => new() { Success = false, ErrorMessage = message };
}