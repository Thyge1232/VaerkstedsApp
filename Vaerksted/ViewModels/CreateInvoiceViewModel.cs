using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Workshop.Data;
using Workshop.Models;
using System.Linq; // Add this for Sum() method
using Microsoft.Maui.Controls; // Added

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
    [ObservableProperty] private double hours;
    [ObservableProperty] private double hourPrice;

    // Materials
    public ObservableCollection<ItemEntry> Items { get; } = new();

    [ObservableProperty] private double materialsSubtotal;
    [ObservableProperty] private string materialName = string.Empty;
    [ObservableProperty] private double laborTotal;
    [ObservableProperty] private double grandTotal;

    private void RecalcTotals()
    {
        MaterialsSubtotal = Items.Sum(i => i.UnitPrice * i.Quantity);
        LaborTotal = Hours * HourPrice;
        GrandTotal = MaterialsSubtotal + LaborTotal;
    }

    partial void OnHoursChanged(double value) => RecalcTotals();
    partial void OnHourPriceChanged(double value) => RecalcTotals();

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
        if (order is null)
        {
            return;
        }

        var invoice = new Invoice
        {
            OrderId = OrderId,
            MechanicName = MechanicName,
            Hours = Hours,
            HourPrice = HourPrice
        };

        await _db.Conn.InsertAsync(invoice);

        foreach (var it in Items)
        {
            var item = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                MaterialName = it.MaterialName,
                UnitPrice = it.UnitPrice,
                Quantity = it.Quantity
            };
            await _db.Conn.InsertAsync(item);
        }

        // clear form after save
        MechanicName = string.Empty;
        Hours = 0;
        HourPrice = 0;
        Items.Clear();
        RecalcTotals();

        // navigate back so the invoices list/page can refresh
        await Shell.Current.GoToAsync("..");
    }

    // Row model
    public partial class ItemEntry : ObservableObject
    {
        [ObservableProperty] private string materialName = string.Empty; // TILF�JET standardv�rdi
        [ObservableProperty] private double unitPrice;
        [ObservableProperty] private int quantity;
    }
}