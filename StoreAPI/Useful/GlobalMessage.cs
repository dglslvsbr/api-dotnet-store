namespace StoreAPI.Useful;

public abstract class GlobalMessage
{
    public static string NotFound404 { get; private set; } = "No data was found.";
    public static string Conflit409 { get; private set; } = "This data already exists.";
    public static string BadRequest400 { get; private set; } = "An error occurred in the request.";
    public static string OK200 { get; private set; } = "Request completed successfully";
    public static string Created201 { get; set; } = "A new feature has been successfully created.";
}