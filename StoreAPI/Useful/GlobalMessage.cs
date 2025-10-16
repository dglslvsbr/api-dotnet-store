namespace StoreAPI.Useful;

public abstract class GlobalMessage
{
    public static string NotFound404 { get; private set; } = "No data found";
    public static string Conflit409 { get; private set; } = "Conflict, data already exists";
    public static string BadRequest400 { get; private set; } = "An error occurred in the request";
    public static string OK200 { get; private set; } = "Request successful";
    public static string Created201 { get; set; } = "A new resource has been successfully created";
    public static string Message(string text) => text;
}