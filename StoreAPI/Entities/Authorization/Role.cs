namespace StoreAPI.Entities.Authentication
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<ClientRole>? ClientRole { get; set; }
    }
}