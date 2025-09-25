using SQLite;

namespace Workshop.Models;

public class InvoiceItem
{
    [PrimaryKey, AutoIncrement]
    public int Id {get; set;}
    public int InvoiceId {get; set;} // Connection to Invoice 
    public string MaterialName {get; set;} = string.Empty;
    public double? UnitPrice {get; set;}
    public int? Quantity {get; set;}

}