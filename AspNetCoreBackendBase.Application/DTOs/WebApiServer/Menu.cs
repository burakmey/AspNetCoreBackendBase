namespace AspNetCoreBackendBase.Application.DTOs
{
    public class Menu
    {
        public required string Route { get; set; }
        public List<string> Codes { get; set; } = [];
    }
}
