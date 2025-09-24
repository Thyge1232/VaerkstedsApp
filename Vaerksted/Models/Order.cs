using SQLite;

namespace Workshop.Models;

public class Order
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string CarMake { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public DateTime DropOffDateTime { get; set; }
    public string WorkDescription { get; set; } = string.Empty;

    public int OrderNumber { get; set; } 
}