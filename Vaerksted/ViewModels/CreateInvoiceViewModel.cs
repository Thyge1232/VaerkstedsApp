using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Workshop.Data;
using Workshop.Models;
using System.Linq; 
using Microsoft.Maui.Controls; 

namespace Workshop.ViewModels;

public partial class CreateInvoiceViewModel : ObservableObject
{
    private readonly AppDatabase _db;

    public CreateInvoiceViewModel(AppDatabase db)
    {
        _db = db;
        Items.CollectionChanged += (_, __) => RecalcTotals();
    }

    // Invoice header fields
    [ObservableProperty] private int orderId;
    [ObservableProperty] private string mechanicName = string.Empty;
    [ObservableProperty] private double? hours;
    [ObservableProperty] private double? hourPrice;

    // Materials
    public ObservableCollection<ItemEntry> Items { get; } = new();

    [ObservableProperty] private double materialsSubtotal;
    [ObservableProperty] private string materialName = string.Empty;
    [ObservableProperty] private double laborTotal;
    [ObservableProperty] private double grandTotal;

    private void RecalcTotals()
    {
        MaterialsSubtotal = Items.Sum(i => (i.UnitPrice ?? 0.0) * (i.Quantity ?? 0));
        LaborTotal = (Hours ?? 0.0) * (HourPrice ?? 0.0);
        GrandTotal = MaterialsSubtotal + LaborTotal;
    }

    partial void OnHoursChanged(double? value) => RecalcTotals();
    partial void OnHourPriceChanged(double? value) => RecalcTotals();

    // Commands
    [RelayCommand]
    private void AddItem()
    {
        var row = new ItemEntry();
        row.PropertyChanged += (_, __) => RecalcTotals();
        Items.Add(row);
    }

    [RelayCommand]
    private void RemoveItem(ItemEntry? row)
    {
        if (row is null) return;
        Items.Remove(row);
        RecalcTotals();
    }

    [RelayCommand]
    private async Task SaveInvoiceAsync()
    {
        // Basic validation
        var order = await _db.Conn.FindAsync<Order>(OrderId);
        if (order is null) return;

        var invoice = new Invoice
        {
            OrderId = OrderId,
            MechanicName = MechanicName,
            Hours = Hours ?? 0.0,
            HourPrice = HourPrice ?? 0.0
        };

        await _db.Conn.InsertAsync(invoice);

        foreach (var it in Items.Where(i => !string.IsNullOrWhiteSpace(i.MaterialName) || i.UnitPrice != null || i.Quantity != null))
        {
            var item = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                MaterialName = it.MaterialName,
                UnitPrice = it.UnitPrice ?? 0.0,
                Quantity = it.Quantity ?? 0
            };
            await _db.Conn.InsertAsync(item);
        }

        // clear celler efter order
        MechanicName = string.Empty;
        Hours = 0;
        HourPrice = 0;
        Hours = null;
        HourPrice = null;
        Items.Clear();
        RecalcTotals();

        
        await Shell.Current.GoToAsync("..");
    }

    // Kode så man kan se hvilken ordre når man laver invoice
    [ObservableProperty] private string orderDisplay = string.Empty;

    public async Task EnsureDefaultOrderAsync()
    {
        if (OrderId != 0)
        {
            await RefreshOrderDisplayAsync();
            return;
        }

        var last = await _db.Conn.Table<Order>()
                      .OrderByDescending(o => o.Id)
                      .FirstOrDefaultAsync();

        if (last is not null)
        {
            OrderId = last.Id;
            OrderDisplay = $"#{last.Id} — {last.CustomerName} ({last.DropOffDateTime:yyyy-MM-dd})";
        }
        else
        {
            OrderDisplay = "No orders available";
        }
    }

    partial void OnOrderIdChanged(int value)
    {
        // fire-and-forget is fine for a quick UI update
        _ = RefreshOrderDisplayAsync();
    }

    public async Task RefreshOrderDisplayAsync()
    {
        if (OrderId == 0)
        {
            OrderDisplay = "No order selected";
            return;
        }

        var order = await _db.Conn.FindAsync<Order>(OrderId);
        OrderDisplay = order is not null
            ? $"#{order.Id} — {order.CustomerName} ({order.DropOffDateTime:yyyy-MM-dd})"
            : $"Order #{OrderId} (not found)";
    }

    // Row model
    public partial class ItemEntry : ObservableObject
    {
        [ObservableProperty] private string materialName = string.Empty; // TILF�JET standardv�rdi
        [ObservableProperty] private double? unitPrice;
        [ObservableProperty] private int? quantity;
    }
}