namespace FiorelloBackend.ViewModels.Home
{
    public class BasketDetailVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
