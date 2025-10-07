namespace StoreAPI.DTOs;

public class ShowCategoryDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ShowProductDTO>? Product { get; set; }
}