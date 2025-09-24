using SQLite;

namespace Workshop.Models;

public class Invoice
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string MechanicName { get; set; } = string.Empty; 
    public double Hours { get; set; }
    public double HourPrice { get; set; }
}