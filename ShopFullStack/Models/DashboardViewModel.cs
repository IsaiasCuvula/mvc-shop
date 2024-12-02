namespace ShopFullStack.Models;

public class DashboardViewModel
{
    public List<Product> PopularProducts { get; set; } = new List<Product>();
    public List<Order> UnpaidOrders { get; set; } = new List<Order>();
    public List<Order> ReturnedOrders { get; set; } = new List<Order>();
    public List<Product> Products { get; set; } = new List<Product>();
    public List<Product> ExpiredProducts { get; set; } = new List<Product>();
    public List<Product> ExpiringSoonProducts { get; set; } = new List<Product>();
    public List<Product> ProductsExpiringInNext3Months { get; set; } = new List<Product>();
    public List<Product> MostPopularProducts { get; set; } = new List<Product>();
    public List<Customer> GetAllCustomerShoppedLasWeek { get; set; } = new List<Customer>();
    public Customer? GetTopCustomerByTurnover { get; set; }
}